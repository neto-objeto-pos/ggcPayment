'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     RetMgtSys Credit Card
'
' Copyright 2016 and Beyond
' All Rights Reserved
' ºººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººº
' €  All  rights reserved. No part of this  software  €€  This Software is Owned by        €
' €  may be reproduced or transmitted in any form or  €€                                   €
' €  by   any   means,  electronic   or  mechanical,  €€    GUANZON MERCHANDISING CORP.    €
' €  including recording, or by information  storage  €€     Guanzon Bldg. Perez Blvd.     €
' €  and  retrieval  systems, without  prior written  €€           Dagupan City            €
' €  from the author.                                 €€  Tel No. 522-1085 ; 522-9275      €
' ºººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººº
'
' ==========================================================================================
'  Jheff [ 10/12/2016 02:58 pm ]
'     Start coding this object...
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
Imports ggcAppDriver
Imports ggcRetailParams
Imports MySql.Data.MySqlClient

Public Class GiftCerticate

#Region "Constant"
    Private Const pxeMODULENAME As String = "GiftCertificate"
    Private Const pxeMasterTble As String = "Gift_Certificate_Trans"
#End Region

#Region "Protected Members"
    Protected p_oAppDrvr As GRider
    Protected p_oCompany As clsCompanyAffiliates
    Protected p_oDataTable As DataTable
    Protected p_nEditMode As xeEditMode
    Protected p_sBranchCd As String
    Protected p_sSourceNo As String
    Protected p_sSourceCd As String
    Protected p_sPOSNo As String

    Protected p_sCompnyNm As String
    Protected p_bCloseForm As Boolean
    Protected p_sTransNox As String
#End Region

#Region "Event"
    Public Event MasterRetrieved(ByVal Row As Integer, _
                                 ByVal Index As Object, _
                                 ByVal Value As Object)
#End Region

#Region "Properties"
    ReadOnly Property AppDriver() As GRider
        Get
            Return p_oAppDrvr
        End Get
    End Property

    Property Branch() As String
        Get
            Return p_sBranchCd
        End Get
        Set(ByVal Value As String)
            p_sBranchCd = Value
        End Set
    End Property

    WriteOnly Property POSNumbr As String
        Set(ByVal Value As String)
            p_sPOSNo = Value
        End Set
    End Property

    ReadOnly Property CloseForm() As Boolean
        Get
            Return p_bCloseForm
        End Get
    End Property

    ReadOnly Property GiftCertTrans() As DataTable
        Get
            Return p_oDataTable
        End Get
    End Property

    Property SourceNo() As String
        Get
            Return p_sSourceNo
        End Get
        Set(ByVal Value As String)
            p_sSourceNo = Value
        End Set
    End Property

    Property SourceCd() As String
        Get
            Return p_sSourceCd
        End Get
        Set(ByVal Value As String)
            p_sSourceCd = Value
        End Set
    End Property

    ReadOnly Property ItemCount() As Long
        Get
            Return p_oDataTable.Rows.Count
        End Get
    End Property

    Property Master(ByVal Row As Integer, _
                    ByVal Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then Index = LCase(Index)
            Select Case Index
                Case "stransnox" : Index = 0
                Case "scompnycd" : Index = 1
                Case "srefernox" : Index = 2
                Case "dvalidity" : Index = 3
                Case "namountxx" : Index = 4
                Case "sremarksx" : Index = 5
                Case "ssourcecd" : Index = 6
                Case "ssourceno" : Index = 7
                Case "ccollectd" : Index = 8
                Case "cbilledxx" : Index = 9
                Case "dbilledxx" : Index = 10
                Case "cpaidxxxx" : Index = 11
                Case "dpaidxxxx" : Index = 12
                Case "cwaivexxx" : Index = 13
                Case "dwaivexxx" : Index = 14
                Case "swaivexxx" : Index = 15
                Case "ctranstat" : Index = 16
                Case "scompnynm" : Index = 17
                    ''Load the company name to the virtual field if virtual field is empty but scompnycd has value...
                    'If Trim(p_oDataTable(Row)("scompnyNm")) = "" And p_oDataTable(Row)("sCompnyCd") <> "" Then
                    '    Dim loRow As DataTable
                    '    loRow = p_oCompany.GetAffiliate(p_oDataTable(Row)("sCompnyCd"), True)
                    '    If Not IsNothing(loRow) Then
                    '        p_oDataTable(Row)("scompnyNm") = loRow.Rows(0)("sCompnyNm")
                    '    End If
                    'End If

                    'Return p_oDataTable(Row)("scompnyNm")
                Case Else
                    MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                    Return DBNull.Value
            End Select
            Return p_oDataTable(Row)(Index)
        End Get

        Set(ByVal Value As Object)
            Select Case Index
                Case "stransnox" : Index = 0
                Case "scompnycd" : Index = 1
                Case "srefernox" : Index = 2
                Case "dValidity" : Index = 3
                    If Not IsDate(Value) Then Value = p_oAppDrvr.SysDate
                    RaiseEvent MasterRetrieved(Row, 3, Value)
                Case "namountxx" : Index = 4
                    If Not IsNumeric(Value) Then Value = 0.0
                    RaiseEvent MasterRetrieved(Row, 4, Value)
                Case "sremarksx" : Index = 5
                Case "ssourcecd" : Index = 6
                Case "ssourceno" : Index = 7
                Case "ccollectd" : Index = 8
                Case "cbilledxx" : Index = 9
                Case "dbilledxx" : Index = 10
                Case "cpaidxxxx" : Index = 11
                Case "dpaidxxxx" : Index = 12
                Case "cwaivexxx" : Index = 13
                Case "dwaivexxx" : Index = 14
                Case "swaivexxx" : Index = 15
                Case "ctranstat" : Index = 16
                Case Else
                    MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
            End Select
            p_oDataTable(Row)(Index) = Value
        End Set
    End Property
#End Region

#Region "Public Function"
    Function SaveTransaction() As Boolean
        Dim lsSQL As String
        Dim lnRow As Integer
        Dim lnCtr As Integer
        Dim lnTotal As Decimal

        With p_oDataTable
            For lnCtr = 0 To .Rows.Count - 1
                If Not isEntryOK(lnCtr) Then Return False

                lsSQL = "INSERT INTO " & pxeMasterTble & " SET" & _
                            "  sTransNox = " & strParm(.Rows(lnCtr)("sTransNox")) & _
                            ", sCompnyCd = " & strParm(.Rows(lnCtr)("sCompnyCd")) & _
                            ", sReferNox = " & strParm(.Rows(lnCtr)("sReferNox")) & _
                            ", dValidity = " & dateParm(.Rows(lnCtr)("dValidity")) & _
                            ", nAmountxx = " & CDec(.Rows(lnCtr)("nAmountxx")) & _
                            ", sRemarksx = " & strParm(.Rows(lnCtr)("sRemarksx")) & _
                            ", sSourceCd = " & strParm(p_sSourceCd) & _
                            ", sSourceNo = " & strParm(p_sSourceNo) & _
                            ", cTranStat = " & strParm(.Rows(lnCtr)("cTranStat")) & _
                            ", dModified = " & dateParm(p_oAppDrvr.SysDate) & _
                        " ON DUPLICATE KEY UPDATE" & _
                            "  sCompnyCd = " & strParm(.Rows(lnCtr)("sCompnyCd")) & _
                            ", sReferNox = " & strParm(.Rows(lnCtr)("sReferNox")) & _
                            ", dValidity = " & dateParm(.Rows(lnCtr)("dValidity")) & _
                            ", nAmountxx = " & CDec(.Rows(lnCtr)("nAmountxx")) & _
                            ", sRemarksx = " & strParm(.Rows(lnCtr)("sRemarksx"))

                Try
                    lnRow = p_oAppDrvr.Execute(lsSQL, pxeMasterTble)
                    If lnRow <= 0 Then
                        MsgBox("Unable to Save Transaction!!!" & vbCrLf & _
                                "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                        Return False
                    End If
                Catch ex As Exception
                    Throw ex
                End Try

                lnTotal += CDec(.Rows(lnCtr)("nAmountxx"))
            Next lnCtr
            p_nGiftCert = lnTotal

            If Not p_sTransNox = String.Empty Then
                If p_oAppDrvr.Execute("DELETE FROM " & pxeMasterTble &
                                                " WHERE sSourceNo = " & strParm(p_sSourceNo) &
                                                    " AND sSourceCd = " & strParm(p_sSourceCd) &
                                                    " AND sTransNox IN(" & p_sTransNox.Substring(0, p_sTransNox.Length - 1) & ")", pxeMasterTble) <= 0 Then
                    'MsgBox("Unable to Save Transaction!!!" & vbCrLf & _
                    '                "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                    'Return False
                End If
            End If
        End With

        Return True
    End Function

    Function SearchCompany(ByVal Row As Integer, _
                           Optional Value As Object = "") As Boolean

        Return getCompany(Row, Value)
    End Function

    Function SearchCompany() As DataTable

        Return p_oCompany.GetAffiliate
    End Function

    Function OpenTransaction(ByVal sTransNox As String) As Boolean
        Dim loDT As New DataTable
        Dim lsSQL As String
        Dim lnCtr As Integer
        Dim lnTotal As Decimal

        lsSQL = AddCondition(getSQL_Master, "a.sTransNox = " & strParm(sTransNox))

        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then Return False

        Call createTable()
        With p_oDataTable
            For lnCtr = 0 To loDT.Rows.Count - 1
                Call AddGiftCert()
                .Rows(lnCtr)("sTransNox") = loDT.Rows(lnCtr)("sTransNox")
                .Rows(lnCtr)("sCompnyCd") = loDT.Rows(lnCtr)("sCompnyCd")
                .Rows(lnCtr)("dValidity") = loDT.Rows(lnCtr)("dValidity")
                .Rows(lnCtr)("nAmountxx") = loDT.Rows(lnCtr)("nAmountxx")
                .Rows(lnCtr)("sRemarksx") = loDT.Rows(lnCtr)("sRemarksx")
                .Rows(lnCtr)("sSourceCd") = loDT.Rows(lnCtr)("sSourceCd")
                .Rows(lnCtr)("sSourceNo") = loDT.Rows(lnCtr)("sSourceNo")
                .Rows(lnCtr)("cCollectd") = loDT.Rows(lnCtr)("cCollectd")
                .Rows(lnCtr)("cBilledxx") = loDT.Rows(lnCtr)("cBilledxx")
                .Rows(lnCtr)("dBilledxx") = loDT.Rows(lnCtr)("dBilledxx")
                .Rows(lnCtr)("cPaidxxxx") = loDT.Rows(lnCtr)("cPaidxxxx")
                .Rows(lnCtr)("dPaidxxxx") = loDT.Rows(lnCtr)("dPaidxxxx")
                .Rows(lnCtr)("cWaivexxx") = loDT.Rows(lnCtr)("cWaivexxx")
                .Rows(lnCtr)("dWaivexxx") = loDT.Rows(lnCtr)("dWaivexxx")
                .Rows(lnCtr)("sWaivexxx") = loDT.Rows(lnCtr)("sWaivexxx")
                .Rows(lnCtr)("cTranStat") = loDT.Rows(lnCtr)("cTranStat")
                .Rows(lnCtr)("sCompnyNm") = loDT.Rows(lnCtr)("sCompnyNm")
                lnTotal += loDT.Rows(lnCtr)("nAmountxx")
            Next lnCtr
            p_nGiftCert = lnTotal
        End With

        Return True
    End Function

    Function OpenBySource() As Boolean
        Dim loDT As New DataTable
        Dim lsSQL As String
        Dim lnCtr As Integer
        Dim lnTotal As Decimal

        lsSQL = AddCondition(getSQL_Master, "a.sSourceNo = " & strParm(p_sSourceNo)) & _
                                                " AND a.sSourceCd = " & strParm(p_sSourceCd)

        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then Return False

        Call createTable()
        With p_oDataTable
            For lnCtr = 0 To loDT.Rows.Count - 1
                .Rows.Add()
                .Rows(lnCtr)("sTransNox") = loDT.Rows(lnCtr)("sTransNox")
                .Rows(lnCtr)("sCompnyCd") = loDT.Rows(lnCtr)("sCompnyCd")
                .Rows(lnCtr)("sReferNox") = loDT.Rows(lnCtr)("sReferNox")
                .Rows(lnCtr)("dValidity") = loDT.Rows(lnCtr)("dValidity")
                .Rows(lnCtr)("nAmountxx") = loDT.Rows(lnCtr)("nAmountxx")
                .Rows(lnCtr)("sRemarksx") = loDT.Rows(lnCtr)("sRemarksx")
                .Rows(lnCtr)("sSourceCd") = loDT.Rows(lnCtr)("sSourceCd")
                .Rows(lnCtr)("sSourceNo") = loDT.Rows(lnCtr)("sSourceNo")
                .Rows(lnCtr)("cCollectd") = loDT.Rows(lnCtr)("cCollectd")
                .Rows(lnCtr)("cBilledxx") = loDT.Rows(lnCtr)("cBilledxx")
                .Rows(lnCtr)("dBilledxx") = loDT.Rows(lnCtr)("dBilledxx")
                .Rows(lnCtr)("cPaidxxxx") = loDT.Rows(lnCtr)("cPaidxxxx")
                .Rows(lnCtr)("dPaidxxxx") = loDT.Rows(lnCtr)("dPaidxxxx")
                .Rows(lnCtr)("cWaivexxx") = loDT.Rows(lnCtr)("cWaivexxx")
                .Rows(lnCtr)("dWaivexxx") = loDT.Rows(lnCtr)("dWaivexxx")
                .Rows(lnCtr)("sWaivexxx") = loDT.Rows(lnCtr)("sWaivexxx")
                .Rows(lnCtr)("cTranStat") = loDT.Rows(lnCtr)("cTranStat")
                .Rows(lnCtr)("sCompnyNm") = loDT.Rows(lnCtr)("sCompnyNm")
                lnTotal += loDT.Rows(lnCtr)("nAmountxx")
            Next lnCtr
            p_nGiftCert = lnTotal
        End With

        Return True
    End Function

    Function DeleteGC(ByVal Row As Integer) As Boolean
        With p_oDataTable
            If .Rows.Count - 1 < Row Then Return False
            p_sTransNox = p_sTransNox & "'" & .Rows(Row)("sTransNox") & "',"
            .Rows(Row).Delete()
        End With

        Return True
    End Function

    Function AddGiftCert() As Boolean
        Dim lnRow As Integer = p_oDataTable.Rows.Count

        If lnRow > 0 Then If Not isEntryOK(lnRow - 1) Then Return False
        If Not SaveTransaction() Then Return False

        With p_oDataTable
            .Rows.Add()
            .Rows(lnRow)("sTransNox") = GetNextCode(pxeMasterTble, "sTransNox", True, p_oAppDrvr.Connection, True, p_sBranchCd)
            .Rows(lnRow)("sCompnyCd") = ""
            .Rows(lnRow)("sReferNox") = ""
            .Rows(lnRow)("dValidity") = p_oAppDrvr.SysDate
            .Rows(lnRow)("sRemarksx") = ""
            .Rows(lnRow)("nAmountxx") = 0.0
            .Rows(lnRow)("sSourceCd") = ""
            .Rows(lnRow)("sSourceNo") = ""
            .Rows(lnRow)("cTranStat") = Val(xeTranStat.TRANS_OPEN)
            .Rows(lnRow)("sCompnyNm") = ""
        End With

        Return True
    End Function
#End Region

#Region "Private function"
    Private Function NewTransaction() As Boolean
        Call createTable()
        Call initMaster()

        Return True
    End Function

    Private Function getSQL_Master() As String
        Return "SELECT" & _
                    "  a.sTransNox" & _
                    ", a.sCompnyCd" & _
                    ", a.sReferNox" & _
                    ", a.dValidity" & _
                    ", a.nAmountxx" & _
                    ", a.sRemarksx" & _
                    ", a.sSourceCd" & _
                    ", a.sSourceNo" & _
                    ", a.cCollectd" & _
                    ", a.cBilledxx" & _
                    ", a.dBilledxx" & _
                    ", a.cPaidxxxx" & _
                    ", a.dPaidxxxx" & _
                    ", a.cWaivexxx" & _
                    ", a.dWaivexxx" & _
                    ", a.sWaivexxx" & _
                    ", a.cTranStat" & _
                    ", b.sCompnyNm" & _
                " FROM " & pxeMasterTble & " a" & _
                    ", Affiliated_Company b" & _
                " WHERE a.sCompnyCd = b.sCompnyCd"
    End Function

    Private Function getCompany(ByVal Row As Integer, _
                                ByVal Value As String) As Boolean
        Dim lsCondition As String
        Dim lsProcName As String
        Dim loDataRow As DataRow

        lsProcName = "getCompany"

        lsCondition = String.Empty

        If Value <> String.Empty Then
            If Value = p_oDataTable(Row)("sCompnyNm") Then Return True
        End If

        loDataRow = p_oCompany.SearchAffiliate(Value, False)
        If Not IsNothing(loDataRow) Then
            p_oDataTable(Row)("sCompnyCd") = loDataRow("sCompnyCd")
            p_oDataTable(Row)("sCompnyNm") = loDataRow("sCompnyNm")

            p_sCompnyNm = loDataRow("sCompnyNm")
            RaiseEvent MasterRetrieved(Row, 1, loDataRow("sCompnyNm"))
        Else
            p_oDataTable(Row)("sCompnyCd") = ""
            p_oDataTable(Row)("sCompnyNm") = ""
            RaiseEvent MasterRetrieved(Row, 1, p_oDataTable(Row)("sCompnyNm"))
        End If

        Return True
    End Function
#End Region

#Region "Private Procedures"
    Private Sub createTable()
        p_oDataTable = New DataTable
        With p_oDataTable
            .Columns.Add("sTransNox", GetType(String)).MaxLength = 20
            .Columns.Add("sCompnyCd", GetType(String)).MaxLength = 8
            .Columns.Add("sReferNox", GetType(String)).MaxLength = 15
            .Columns.Add("dValidity", GetType(Date))
            .Columns.Add("nAmountxx", GetType(Decimal))
            .Columns.Add("sRemarksx", GetType(String)).MaxLength = 64
            .Columns.Add("sSourceCd", GetType(String)).MaxLength = 4
            .Columns.Add("sSourceNo", GetType(String)).MaxLength = 20
            .Columns.Add("cCollectd", GetType(String)).MaxLength = 1
            .Columns.Add("cBilledxx", GetType(String)).MaxLength = 1
            .Columns.Add("dBilledxx", GetType(Date))
            .Columns.Add("cPaidxxxx", GetType(String)).MaxLength = 1
            .Columns.Add("dPaidxxxx", GetType(Date))
            .Columns.Add("cWaivexxx", GetType(String)).MaxLength = 1
            .Columns.Add("dWaivexxx", GetType(Date))
            .Columns.Add("sWaivexxx", GetType(String)).MaxLength = 12
            .Columns.Add("cTranStat", GetType(String)).MaxLength = 1
            .Columns.Add("sCompnyNm", GetType(String)).MaxLength = 64
        End With
    End Sub

    Private Sub initMaster()
        With p_oDataTable
            .Rows.Add()
            .Rows(0)("sTransNox") = getNextTransNo()
            .Rows(0)("sCompnyCd") = ""
            .Rows(0)("sReferNox") = ""
            .Rows(0)("dValidity") = p_oAppDrvr.SysDate
            .Rows(0)("sRemarksx") = ""
            .Rows(0)("nAmountxx") = 0.0
            .Rows(0)("sSourceCd") = p_sSourceCd
            .Rows(0)("sSourceNo") = p_sSourceNo
            .Rows(0)("cTranStat") = Val(xeTranStat.TRANS_OPEN)
            .Rows(0)("sCompnyNm") = ""
        End With
    End Sub

    Private Function isEntryOK(ByVal fnRowNo As Integer) As Boolean
        ' verify the required fields
        If p_oDataTable.Rows(fnRowNo)("sCompnyCd") = String.Empty Then
            MsgBox("Invalid Transaction Company Detected!!!" & vbCrLf & vbCrLf & _
                   "Verify your Entry then Try Again!!!", vbCritical, "Warning")
            Return False
        End If

        If p_oDataTable.Rows(fnRowNo)("nAmountxx") <= 0.0 Then
            MsgBox("Invalid Transaction Check Amount Detected!!!" & vbCrLf & vbCrLf & _
                   "Verify your Entry then Try Again!!!", vbCritical, "Warning")
            Return False
        End If

        Return True
    End Function
#End Region

#Region "Public Procedures"
    Sub ShowGiftCert()
        p_oFormGC = New frmPayGC
        With p_oFormGC
            .GiftCert = Me
            .TopMost = True
            .ShowDialog()
            p_bCloseForm = .CloseForm
        End With
    End Sub
#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub New(ByVal foRider As GRider)
        p_oAppDrvr = foRider
        If p_sBranchCd = String.Empty Then p_sBranchCd = p_oAppDrvr.BranchCode
        p_oCompany = New clsCompanyAffiliates(p_oAppDrvr, False)
        
        Call NewTransaction()

        p_nEditMode = xeEditMode.MODE_UNKNOWN
    End Sub

    Private Function getNextTransNo() As String
        Dim loDA As New MySqlDataAdapter
        Dim loDT As New DataTable
        Dim loDS As New DataSet
        Dim lsSQL As String
        Dim lnCounter As Integer
        Dim lnCode As Long
        Dim lnLen As Long
        Dim lsStr As String = ""

        lsSQL = "SELECT sTransNox" & _
                " FROM " & pxeMasterTble & _
                " WHERE sTransNox LIKE " & strParm(p_sBranchCd & p_sPOSNo & Format(p_oAppDrvr.getSysDate(), "yy") & "%") & _
                " ORDER BY sTransNox DESC" & _
                " LIMIT 1"

        Try
            loDA.SelectCommand = New MySqlCommand(lsSQL, p_oAppDrvr.Connection)
        Catch ex As MySqlException
            MsgBox(ex.Message)
            Throw ex
        End Try

        loDT.Clear()
        loDA.Fill(loDT)
        If loDT.Rows.Count = 0 Then
            lsSQL = ""

            loDA.FillSchema(loDS, SchemaType.Source)
            lnLen = loDS.Tables(0).Columns(0).MaxLength
            lnCode = 1

            lsSQL = p_sBranchCd & p_sPOSNo & Format(p_oAppDrvr.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        Else
            lsSQL = p_sBranchCd & p_sPOSNo & Format(p_oAppDrvr.getSysDate(), "yy")
            lnCounter = Len(lsSQL)

            lsSQL = loDT.Rows(0).Item("sTransNox")
            lnLen = Len(lsSQL)

            lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
        End If

        If lsSQL = "" Then
            lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
        Else
            lsSQL = p_sBranchCd & p_sPOSNo & Format(p_oAppDrvr.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        End If

        If lsSQL = "" Then
            Return Format(lnCode, lsStr.PadRight(lnCounter, "0"))
        Else
            Return Left(lsSQL, lnCounter) & Format(lnCode, lsStr.PadRight(lnLen - lnCounter, "0"))
        End If
    End Function
End Class