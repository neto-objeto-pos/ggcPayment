'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     Robinson Land Corporation (RLC) Writing Object
'
' Copyright 2021 and Beyond
' All Rights Reserved
' ºººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººº
' €  All  rights reserved. No part of this  software  €€  This Software is Owned by        €
' €  may be reproduced or transmitted in any form or  €€                                   €
' €  by   any   means,  electronic   or  mechanical,  €€    GUANZON MERCHANDISING CORP.    €
' €  including recording, or by information  storage  €€     Guanzon Bldg. Perez Blvd.     €
' €  and  retrieval  systems, without  prior written  €€           Dagupan City            €
' €  from the author.                                 €€  Tel No. 522-1085 ; 522-9275      €
' ºººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººº
' ==========================================================================================
'  Jeff [ 07/16/2021 09:03 am ]
'      Started creating this object.
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
Imports ggcAppDriver
Imports System.IO
Imports System.Net
Imports WinSCP

Public Class PRN_RLC_Reading
    Private p_oApp As GRider

    Private p_oDTMaster As DataTable
    Private p_bBackEnd As Boolean = False

    Private Const pxeLFTMGN As Integer = 3

    Private p_sPOSNo As String
    Private p_sTenantID As String
    Private p_sTermnlID As String
    Private p_sHostName As String
    Private p_sUserName As String
    Private p_sPassword As String

    Private Const p_sMasTable As String = "Daily_Summary"
    Private Const p_sMsgHeadr As String = "Daily Summary"

    WriteOnly Property isBackend() As Boolean
        Set(ByVal value As Boolean)
            p_bBackEnd = value
        End Set
    End Property

    Public Function generateRLC(ByVal sFromDate As String, ByVal sThruDate As String, ByVal sCRMNumbr As String) As Boolean
        'Get configuration of machine
        Call doWriteRLCReading(sFromDate, sCRMNumbr, 1)
        Call uploadSFTPUnsent(Environment.GetEnvironmentVariable("RLC-XPATH"))
        Call uploadSFTPneo(Environment.GetEnvironmentVariable("RLC-EPATH"))

        Return True
    End Function

    Public Function createRLC(ByVal sFromDate As String, ByVal sThruDate As String, ByVal sCRMNumbr As String) As Boolean
        'Get configuration of machine
        Call doWriteRLCReading(sFromDate, sCRMNumbr, 1)

        Return True
    End Function

    'Private Function uploadFTP(sPathName As String, sFileName As String) As Boolean
    '    Dim lbStatus As Boolean

    '    'Create Request To Upload File'
    '    Dim wrUpload As FtpWebRequest = DirectCast(WebRequest.Create _
    '        ("ftp://ftp.test.com/file.txt"), FtpWebRequest)

    '    'Specify Username & Password'
    '    wrUpload.Credentials = New NetworkCredential("user",
    '       "password")

    '    'Start Upload Process'
    '    wrUpload.Method = WebRequestMethods.Ftp.UploadFile

    '    Dim response As FtpWebResponse = wrUpload.GetResponse

    '    'Locate File And Store It In Byte Array'
    '    Dim btfile() As Byte = File.ReadAllBytes(sFileName)

    '    Try
    '        'Get File'
    '        Dim strFile As Stream = wrUpload.GetRequestStream()

    '        'Upload Each Byte'
    '        strFile.Write(btfile, 0, btfile.Length)

    '        'Close'
    '        strFile.Close()

    '        'Free Memory'
    '        strFile.Dispose()
    '    Catch ex As Exception
    '        If response.StatusCode = FtpStatusCode.CommandOK Then
    '            My.Computer.FileSystem.MoveFile(sPathName & "\" & sFileName, Environment.GetEnvironmentVariable("RLC-UPATH") & "\" & sFileName)
    '            lbStatus = True
    '        Else
    '            My.Computer.FileSystem.CopyFile(sPathName & "\" & sFileName, sPathName & "\" & sFileName.Substring(0, Len(sFileName) - 1) & Right(sFileName, 1) + 1, overwrite:=False)
    '            My.Computer.FileSystem.MoveFile(sPathName & "\" & sFileName, Environment.GetEnvironmentVariable("RLC-XPATH") & "\" & sFileName)
    '            lbStatus = False
    '        End If
    '        response.Close()
    '    End Try

    '    Return lbStatus
    'End Function

    Private Function uploadSFTPneo(sPathName As String) As Boolean
        If (Directory.EnumerateFiles(sPathName).Count = 0) Then
            Return 0
        End If

        Try
            ' Setup session options
            Dim sessionOptions As New SessionOptions
            With sessionOptions
                .Protocol = Protocol.Sftp
                .HostName = p_sHostName
                .UserName = p_sUserName
                .Password = p_sPassword
                .PortNumber = 22
                '.SshHostKeyFingerprint = "e6vFs7ULtSiKo3GfwWlkuz792QGW2YeyWn/83Vsq38A="
                .SshHostKeyFingerprint = "ssh-ed25519 255 e6vFs7ULtSiKo3GfwWlkuz792QGW2YeyWn/83Vsq38A="
            End With

            Using session As New Session
                ' Connect
                session.DisableVersionCheck = True
                session.SessionLogPath = "D:\rlc\sftplog.log"
                session.Open(sessionOptions)

                ' Upload files
                Dim transferOptions As New TransferOptions
                transferOptions.TransferMode = TransferMode.Binary

                Dim transferResult As TransferOperationResult
                transferResult = session.PutFiles(sPathName & "*", "/50080729/", False, transferOptions)

                ' Throw on any error
                transferResult.Check()

                ' Print results
                For Each transfer In transferResult.Transfers
                    My.Computer.FileSystem.MoveFile(transfer.FileName.ToString, Environment.GetEnvironmentVariable("RLC-UPATH") & transfer.FileName.ToString.Substring(13, 12))
                Next
                MsgBox("Sales file succesfully sent to RLC server.", MsgBoxStyle.Information, "Notice")
            End Using

            Return 0
        Catch e As Exception
            Console.WriteLine("Error: {0}", e)
            Dim dir As New DirectoryInfo(sPathName)

            For Each sfile In dir.GetFiles()
                '    My.Computer.FileSystem.CopyFile(sPathName & sfile.Name, Environment.GetEnvironmentVariable("RLC-NPATH") & sfile.Name.Substring(0, Len(sfile.Name) - 1) & Right(sfile.Name, 1) + 1, overwrite:=False)
                My.Computer.FileSystem.MoveFile(sPathName & sfile.Name, Environment.GetEnvironmentVariable("RLC-XPATH") & sfile.Name)
            Next
            MsgBox(e.Message & vbCrLf &
                    "Sales file not sent to RLC server." & vbCrLf &
                    "Please contact your POS vendor.", MsgBoxStyle.Critical, "WARNING")
            Return 1
        End Try
    End Function

    Public Function uploadSFTPUnsent(sPathName As String) As Boolean
        If (Directory.EnumerateFiles(sPathName).Count = 0) Then
            Return 0
        End If

        Try
            'Setup Session options
            Dim sessionOptions As New SessionOptions
            With sessionOptions
                .Protocol = Protocol.Sftp
                .HostName = p_sHostName
                .UserName = p_sUserName
                .Password = p_sPassword
                .PortNumber = 22
                '.SshHostKeyFingerprint = "e6vFs7ULtSiKo3GfwWlkuz792QGW2YeyWn/83Vsq38A="
                .SshHostKeyFingerprint = "ssh-ed25519 255 e6vFs7ULtSiKo3GfwWlkuz792QGW2YeyWn/83Vsq38A="
            End With

            Using session As New Session
                ' Connect
                session.DisableVersionCheck = True
                session.SessionLogPath = "D:\rlc\sftplog.log"
                session.Open(sessionOptions)

                ' Upload files
                Dim transferOptions As New TransferOptions
                transferOptions.TransferMode = TransferMode.Binary

                Dim transferResult As TransferOperationResult
                transferResult = session.PutFiles(sPathName & "*", "/50080729/", False, transferOptions)

                ' Throw on any error
                transferResult.Check()
                ' Print results
                For Each transfer In transferResult.Transfers
                    My.Computer.FileSystem.MoveFile(transfer.FileName.ToString, Environment.GetEnvironmentVariable("RLC-UPATH") & transfer.FileName.ToString.Substring(13, 13))
                Next
                MsgBox("Sales file succesfully sent to RLC server.", MsgBoxStyle.Information, "Notice")
            End Using

            Return 0
        Catch e As Exception
            Console.WriteLine("Error: {0}", e)
            'Dim dir As New DirectoryInfo(sPathName)

            'For Each sfile In dir.GetFiles()
            '    My.Computer.FileSystem.CopyFile(sPathName & sfile.Name, Environment.GetEnvironmentVariable("RLC-NPATH") & sfile.Name.Substring(0, Len(sfile.Name) - 1) & Right(sfile.Name, 1) + 1, overwrite:=False)
            '    My.Computer.FileSystem.MoveFile(sPathName & sfile.Name, Environment.GetEnvironmentVariable("RLC-XPATH") & sfile.Name)
            'Next
            MsgBox(e.Message & vbCrLf &
                    "Sales file not sent to RLC server." & vbCrLf &
                    "Please contact your POS vendor.", MsgBoxStyle.Critical, "WARNING")
            Return 1
        End Try
    End Function

    'Public Function uploadSFTPbatch(sPathName As String) As Boolean
    '    If (Directory.EnumerateFiles(sPathName).Count = 0) Then
    '        Return 0
    '    End If

    '    Try
    '        ' Setup session options
    '        Dim sessionOptions As New SessionOptions
    '        With sessionOptions
    '            .Protocol = Protocol.Sftp
    '            .HostName = p_sHostName
    '            .UserName = p_sUserName
    '            .Password = p_sPassword
    '            .PortNumber = 22
    '            .SshHostKeyFingerprint = "e6vFs7ULtSiKo3GfwWlkuz792QGW2YeyWn/83Vsq38A="
    '        End With

    '        Using session As New Session
    '            ' Connect
    '            session.Open(sessionOptions)

    '            ' Upload files
    '            Dim transferOptions As New TransferOptions
    '            transferOptions.TransferMode = TransferMode.Binary

    '            Dim transferResult As TransferOperationResult
    '            transferResult = session.PutFiles(sPathName & "*", "/IT_Tenants/", False, transferOptions)

    '            ' Throw on any error
    '            transferResult.Check()

    '            ' Print results
    '            For Each transfer In transferResult.Transfers
    '                My.Computer.FileSystem.MoveFile(transfer.FileName.ToString, Environment.GetEnvironmentVariable("RLC-UPATH") & transfer.FileName.ToString.Substring(16, 12))
    '            Next
    '            MsgBox("Trying to send unsent files…successful.", MsgBoxStyle.Information, "Notice")
    '        End Using

    '        Return 0
    '    Catch e As Exception
    '        Console.WriteLine("Error: {0}", e)
    '        Dim dir As New DirectoryInfo(sPathName)

    '        For Each sfile In dir.GetFiles()
    '            My.Computer.FileSystem.CopyFile(sPathName & sfile.Name, Environment.GetEnvironmentVariable("RLC-NPATH") & sfile.Name.Substring(0, Len(sfile.Name) - 1) & Right(sfile.Name, 1) + 1, overwrite:=False)
    '            My.Computer.FileSystem.MoveFile(sPathName & sfile.Name, Environment.GetEnvironmentVariable("RLC-XPATH") & sfile.Name)
    '        Next
    '        MsgBox("Sales file not sent to RLC server." & vbCrLf &
    '                "Please contact your POS vendor.", MsgBoxStyle.Critical, "WARNING")
    '        Return 1
    '    End Try
    'End Function

    Public Function uploadSFTPFile(sFileName As String) As Boolean
        Dim lsBatchNo As String

        lsBatchNo = ""
        'For Each sfile In Directory.GetFiles(Environment.GetEnvironmentVariable("RLC-NPATH"), sFileName & ".*")
        '    Dim information = My.Computer.FileSystem.GetFileInfo(sfile)
        '    lsBatchNo = information.Name.Substring(11, 1)
        'Next

        If lsBatchNo = "" Then
            For Each sfile In Directory.GetFiles(Environment.GetEnvironmentVariable("RLC-XPATH"), sFileName & ".*")
                Dim information = My.Computer.FileSystem.GetFileInfo(sfile)
                lsBatchNo = information.Name.Substring(11, 1)
            Next
        End If

        If lsBatchNo = "" Then
            For Each sfile In Directory.GetFiles(Environment.GetEnvironmentVariable("RLC-UPATH"), sFileName & ".*")
                Dim information = My.Computer.FileSystem.GetFileInfo(sfile)
                lsBatchNo = information.Name.Substring(11, 1)
            Next
        End If

        If lsBatchNo = "" Then
            MsgBox("No Record found." & vbCrLf &
                    "Pleased verify your entry then try again.", MsgBoxStyle.Critical, "WARNING")
            Return False
        End If

        My.Computer.FileSystem.CopyFile(Environment.GetEnvironmentVariable("RLC-BPATH") & sFileName & "." & p_sTermnlID & "1", Environment.GetEnvironmentVariable("RLC-EPATH") & sFileName & "." & p_sTermnlID & CInt(lsBatchNo) + 1, overwrite:=False)
        Call uploadSFTPneo(Environment.GetEnvironmentVariable("RLC-EPATH"))
        Return True
    End Function

    Private Function doWriteRLCReading(ByVal sDate As String, ByVal sCRMNumbr As String, ByVal nBatchNo As Integer) As Boolean
        Dim lsSQL As String
        lsSQL = AddCondition(getSQ_Master, "sTranDate = " & strParm(sDate) &
                                      " AND sCRMNumbr = " & strParm(sCRMNumbr) &
                                      " AND cTranStat IN ('2')")

        Dim loDta As DataTable
        Dim loCRM As DataTable

        loDta = p_oApp.ExecuteQuery(lsSQL)

        lsSQL = "SELECT nAccuSale, nZReadCtr FROM Daily_Summary" &
        " WHERE sTranDate < " & strParm(sDate) &
            " AND sCRMNumbr = " & strParm(sCRMNumbr) &
            " AND cTranStat IN ('2')" &
        " ORDER BY dClosedxx DESC LIMIT 1"

        Dim loDT As DataTable
        Dim lnPrevSale As Decimal
        Dim lnZReadCtr As Integer

        loDT = p_oApp.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then
            lnPrevSale = 0
            lnZReadCtr = 0
        Else
            lnPrevSale = loDT(0)("nAccuSale")
            lnZReadCtr = loDT(0)("nZReadCtr")
        End If

        lsSQL = "SELECT * FROM Cash_Reg_Machine" &
                " WHERE sIDNumber = " & strParm(sCRMNumbr)
        loCRM = p_oApp.ExecuteQuery(lsSQL)

        Dim builder As New System.Text.StringBuilder()
        Dim lnTotalVat As Decimal

        lnTotalVat = ((loDta.Rows(0)("nSalesAmt") - loDta.Rows(0)("nVatDiscx")) - loDta.Rows(0)("nPWDDiscx") - loDta.Rows(0)("nNonVATxx")) / 1.12 * 0.12
        builder.Append("01" & p_sTenantID.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("02" & p_sTermnlID.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("03" & CDec(FormatNumber(loDta.Rows(0)("nSalesAmt") - loDta.Rows(0)("nVatDiscx"), 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("04" & CDec(FormatNumber(lnTotalVat, 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("05" & CDec(FormatNumber(loDta.Rows(0)("nVoidAmnt"), 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("06" & loDta.Rows(0)("nVoidCntx").ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("07" & CDec(FormatNumber(loDta.Rows(0)("nDiscount"), 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("08" & loDta.Rows(0)("nTotlDisc").ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("09" & CDec(FormatNumber(loDta.Rows(0)("nReturnsx"), 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("10" & loDta.Rows(0)("nTotlRtrn").ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("11" & CDec(FormatNumber(loDta.Rows(0)("nPWDDiscx"), 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("12" & loDta.Rows(0)("nTotSCPWD").ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("13" & CDec(FormatNumber(loDta.Rows(0)("nSChargex"), 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        'builder.Append("14" & ("0").ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("14" & (IIf(loCRM.Rows(0)("nEODCtrxx") = 0, 0, loCRM.Rows(0)("nEODCtrxx") - 1)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("15" & CDec(FormatNumber(lnPrevSale, 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        'builder.Append("16" & "0".ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("16" & loCRM.Rows(0)("nEODCtrxx").ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("17" & CDec(FormatNumber(((loDta.Rows(0)("nSalesAmt") - loDta.Rows(0)("nVatDiscx")) - loDta.Rows(0)("nDiscount") - loDta.Rows(0)("nPWDDiscx")) + lnPrevSale, 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("18" & Format(CDate(loDta.Rows(0)("sTranDate").ToString.Substring(0, 4) & "/" & loDta.Rows(0)("sTranDate").ToString.Substring(4, 2) & "/" & loDta.Rows(0)("sTranDate").ToString.Substring(6, 2)), "MM/dd/yyyy").PadLeft(16, "0") & Environment.NewLine)
        builder.Append("19" & CDec(FormatNumber(0, 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("20" & CDec(FormatNumber(0, 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("21" & CDec(FormatNumber(0, 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("22" & CDec(FormatNumber(loDta.Rows(0)("nCrdtAmnt"), 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("23" & CDec(FormatNumber(loDta.Rows(0)("nCrdtAmnt") / 1.12 * 0.12, 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("24" & CDec(FormatNumber(loDta.Rows(0)("nNonVATxx"), 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("25" & CDec(FormatNumber(0, 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("26" & CDec(FormatNumber(0, 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("27" & CDec(FormatNumber(0, 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("28" & CDec(FormatNumber(0, 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("29" & CDec(FormatNumber(loDta.Rows(0)("nRepAmntx"), 2)).ToString.PadLeft(16, "0") & Environment.NewLine)
        builder.Append("30" & loDta.Rows(0)("nReprintx").ToString.PadLeft(16, "0"))
        RawPrint.writeToRLC(p_sTenantID.Substring(Len(p_sTenantID) - 4, 4) &
                            loDta.Rows(0)("sTranDate").ToString.Substring(4, 2) &
                            loDta.Rows(0)("sTranDate").ToString.Substring(6, 2) &
                            "." & p_sTermnlID.PadLeft(2, "0") &
                            nBatchNo, builder.ToString())
        Return True
    End Function

    Private Function initMachine() As Boolean
        If p_sPOSNo = "" Then
            MsgBox("Invalid Machine Identification Info Detected...")
            Return False
        End If

        Dim lsSQL As String
        lsSQL = "SELECT" &
                       "  sAccredtn" &
                       ", sPermitNo" &
                       ", sSerialNo" &
                       ", nPOSNumbr" &
                       ", nZReadCtr" &
                       ", sRLCHostx" &
                       ", sRLCUserx" &
                       ", sRLCPassx" &
               " FROM Cash_Reg_Machine" &
               " WHERE sIDNumber = " & strParm(p_sPOSNo)

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count <> 1 Then
            MsgBox("Invalid Config for MIN Detected...1")
            Return False
        End If

        p_sTermnlID = loDta.Rows(0)("nPOSNumbr")
        p_sHostName = loDta.Rows(0)("sRLCHostx")
        p_sUserName = loDta.Rows(0)("sRLCUserx")
        p_sPassword = loDta.Rows(0)("sRLCPassx")
        Return True
    End Function

    Private Function PadCenter(source As String, length As Integer) As String
        Dim spaces As Integer = length - source.Length
        Dim padLeft As Integer = spaces / 2 + source.Length
        Return source.PadLeft(padLeft, " ").PadRight(length, " ")
    End Function

    Private Function getSQ_Master() As String
        Return "SELECT a.sTranDate" &
                    ", a.sCRMNumbr" &
                    ", a.sCashierx" &
                    ", a.nOpenBalx" &
                    ", a.nCPullOut" &
                    ", a.nSalesAmt" &
                    ", a.nVATSales" &
                    ", a.nVATAmtxx" &
                    ", a.nNonVATxx" &
                    ", a.nZeroRatd" &
                    ", a.nDiscount" &
                    ", a.nPWDDiscx" &
                    ", a.nVatDiscx" &
                    ", a.nReturnsx" &
                    ", a.nVoidAmnt" &
                    ", a.nAccuSale" &
                    ", a.nCashAmnt" &
                    ", a.nChckAmnt" &
                    ", a.nCrdtAmnt" &
                    ", a.nChrgAmnt" &
                    ", a.nSChargex" &
                    ", a.nRepAmntx" &
                    ", a.nCancelld" &
                    ", a.sORNoFrom" &
                    ", a.sORNoThru" &
                    ", a.nZReadCtr" &
                    ", a.nGiftAmnt" &
                    ", a.cTranStat" &
                    ", a.nVoidCntx" &
                    ", a.nTotlDisc" &
                    ", a.nTotSCPWD" &
                    ", a.nTotlRtrn" &
                    ", a.nTotlCncl" &
                    ", a.nReprintx" &
                " FROM " & p_sMasTable & " a" &
                " ORDER BY a.sTranDate ASC"
    End Function

    Public Sub New(ByVal foRider As GRider)
        p_oApp = foRider

        p_oDTMaster = Nothing

        p_sPOSNo = Environment.GetEnvironmentVariable("RMS-CRM-No")         'POS
        p_sTenantID = Environment.GetEnvironmentVariable("RMS-TENANT-ID")   'Tenant ID

        Call initMachine()
    End Sub

    Public Function IsConnectionAvailable() As Boolean
        Try
            If My.Computer.Network.Ping("www.google.com") Then
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try

        Return False
    End Function

    Public Sub showRLC()
        Dim loFormRLC As frmRLC

        loFormRLC = New frmRLC(p_oApp)
        loFormRLC.RLC = Me
        loFormRLC.ShowDialog()
    End Sub

    Public Function resendFile(sFileName As String) As Boolean
        If Not IsConnectionAvailable() Then
            MsgBox("No Connection." & vbCrLf &
                    "Pleased try againg later.", MsgBoxStyle.Information, "Notice")
            Return False
        End If

        Return uploadSFTPFile(p_sTenantID.Substring(Len(p_sTenantID) - 4, 4) & sFileName)
    End Function
End Class