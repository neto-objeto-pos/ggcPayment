
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     MC AR Master Object
'
' Copyright 2012 and Beyond
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
'  Maynard [ 12/23/2012 11:00 am ]
'      Started creating this object.
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
Imports MySql.Data.MySqlClient
Imports ADODB
Imports ggcAppDriver
Imports ggcClient
Imports CrystalDecisions.CrystalReports.Engine
Imports ggcRMSReports
Imports ggcAppDriver.ModMain

Public Class clsSOA
    Private p_oApp As GRider
    Private p_oDTMaster As DataTable
    'Private p_oDTDetail As DataTable
    Private p_oBillDetail As DataTable
    Private p_oSTRept As DataSet
    Private p_oFormxx As frmReportViewer
    Private p_oReport As ReportDocument

    Private p_oClient As ggcClient.Client
    Private p_nEditMode As xeEditMode
    Private p_oOthersx As New Others
    Private p_nTranStat As Int32  'Transaction Status of the transaction to retrieve
    Private p_sBranchCD As String
    Private p_nEntryCount As Integer
    Private pbModified As Boolean


    Private Const p_sMasTable As String = "Billing_Master"
    Private Const p_sDetTable As String = "Billing_Detail"
    Private Const p_sMsgHeadr As String = "Statement of Account's"

    Public Event MasterRetrieved(ByVal Index As Integer,
                                  ByVal Value As Object)
    Public Event DetailBillRetrieved(ByVal Row As Integer, ByVal Index As Integer,
                              ByVal Value As Object)

    Public Property ReportSource() As ReportDocument
        Get
            Return p_oReport
        End Get
        Set(ByVal foValue As ReportDocument)
            p_oReport = foValue
        End Set
    End Property

    Public ReadOnly Property isModified() As Boolean
        Get
            Return pbModified
        End Get

    End Property
    Public ReadOnly Property AppDriver() As ggcAppDriver.GRider
        Get
            Return p_oApp
        End Get
    End Property

    Public Property Branch As String
        Get
            Return p_sBranchCD
        End Get
        Set(ByVal value As String)
            'If Product ID is LR then do allow changing of Branch
            If p_oApp.ProductID = "TeleMktg" Then
                p_sBranchCD = value
            End If
        End Set
    End Property

    Public Property Master(ByVal Index As Integer) As Object
        Get
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index
                    Case 2 ' sClientNm
                        If Trim(IFNull(p_oDTMaster(0).Item(2))) <> "" And Trim(p_oOthersx.sClientNm) = "" Then
                            getClient(2, 2, p_oDTMaster(0).Item(2), True, False)
                        End If
                        Return p_oOthersx.sClientNm
                    Case 80 ' sAddressx
                        Return p_oOthersx.sAddressx

                    Case Else
                        Return p_oDTMaster(0).Item(Index)
                End Select
            Else
                Return vbEmpty
            End If
        End Get

        Set(ByVal value As Object)
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index
                    Case 6
                        If value <> "" Then
                            p_oDTMaster(0).Item(Index) = value
                            RaiseEvent MasterRetrieved(Index, p_oDTMaster(0).Item(Index))
                        End If
                    Case Else
                        p_oDTMaster(0).Item(Index) = value
                End Select

            End If
        End Set
    End Property

    'Property Master(String)
    Public Property Master(ByVal Index As String) As Object
        Get
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index
                    Case "sclientnm"
                        If Trim(IFNull(p_oDTMaster(0).Item(2))) <> "" And Trim(p_oOthersx.sClientNm) = "" Then
                            getClient(2, 2, p_oDTMaster(0).Item(2), True, False)
                        End If
                        Return p_oOthersx.sClientNm
                    Case "saddressx" ' 

                        Return p_oOthersx.sAddressx
                    Case Else
                        Return p_oDTMaster(0).Item(Index)
                End Select
            Else
                Return vbEmpty
            End If
        End Get
        Set(ByVal value As Object)
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index
                    Case "sremarksx"
                        If value <> "" Then
                            p_oDTMaster(0).Item(Index) = value
                            RaiseEvent MasterRetrieved(Index, p_oDTMaster(0).Item(Index))
                        End If
                    Case Else
                        p_oDTMaster(0).Item(Index) = value
                End Select
            End If
        End Set
    End Property

    Public Property BillDetail(ByVal Row As Integer, ByVal Index As Integer) As Object
        Get
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index

                    Case Else
                        Return p_oBillDetail(Row).Item(Index)
                End Select
            Else
                Return vbEmpty
            End If
        End Get
        Set(ByVal value As Object)
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index
                    Case 5
                        p_oBillDetail(Row).Item(Index) = value
                        If p_oBillDetail(Row).Item(Index) = 1 Then
                            pbModified = True
                            p_nEntryCount += p_nEntryCount
                            'AddDetail(Row)
                        Else
                            pbModified = True
                            p_nEntryCount -= p_nEntryCount
                            'RemoveDetail(p_oBillDetail(Row).Item(0))

                        End If
                        RaiseEvent DetailBillRetrieved(Row, Index, p_oBillDetail(Row).Item(Index))
                    Case 10
                        pbModified = True

                        Debug.Print(" sourceno: " + p_oBillDetail(Row).Item(2))

                        p_oBillDetail(Row).Item(Index) = value
                        RaiseEvent DetailBillRetrieved(Row, Index, p_oBillDetail(Row).Item(Index))
                End Select
            End If
        End Set

    End Property

    'Property Detail(String)
    Public Property BillDetail(ByVal Row As Integer, ByVal Index As String) As Object
        Get
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case LCase(Index)

                    Case Else
                        Return p_oBillDetail(Row).Item(Index)
                End Select
            Else
                Return vbEmpty
            End If
        End Get
        Set(ByVal value As Object)
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index
                    Case "cbilledxx"
                        p_oBillDetail(Row).Item(Index) = value
                        If p_oBillDetail(Row).Item(Index) = 1 Then
                            p_nEntryCount += p_nEntryCount
                            'AddDetail(Row)
                        Else
                            p_nEntryCount -= p_nEntryCount
                            'RemoveDetail(p_oBillDetail(Row).Item(0))
                        End If

                        RaiseEvent DetailBillRetrieved(Row, Index, p_oBillDetail(Row).Item(Index))
                    Case "cpaidxxxx"
                        pbModified = True
                        Debug.Print("Fucking sourceno: " + p_oBillDetail(Row).Item(0))

                        p_oBillDetail(Row).Item(Index) = value
                        RaiseEvent DetailBillRetrieved(Row, Index, p_oBillDetail(Row).Item(Index))
                End Select
            End If
        End Set
    End Property
    'Property EditMode()
    Public ReadOnly Property EditMode() As xeEditMode
        Get
            Return p_nEditMode
        End Get
    End Property

    Public Function GetItemDSCount() As Integer
        If p_oBillDetail Is Nothing Then Return 0

        Return p_oBillDetail.Rows.Count
    End Function

    'Public Function NewTransaction()
    Public Function NewTransaction() As Boolean
        Dim lsSQL As String
        p_nEntryCount = 0
        lsSQL = AddCondition(getSQ_Master, "0=1")
        p_oDTMaster = p_oApp.ExecuteQuery(lsSQL)
        p_oDTMaster.Rows.Add(p_oDTMaster.NewRow())


        Call initMaster()
        Call InitOthers()

        p_nEditMode = xeEditMode.MODE_ADDNEW

        Return True
    End Function


    'Public Function SearchTransaction(String, Boolean, Boolean=False)
    Public Function SearchTransaction(
                        ByVal fsValue As String _
                      , Optional ByVal fbByCode As Boolean = False) As Boolean

        Dim lsSQL As String

        'Check if already loaded base on edit mode
        If p_nEditMode = xeEditMode.MODE_READY Or p_nEditMode = xeEditMode.MODE_UPDATE Then

            If fbByCode Then
                If fsValue = p_oDTMaster(0).Item("sTransNox") Then Return True
            Else
                If fsValue = p_oOthersx.sClientNm Then Return True
            End If
        End If

        'Initialize SQL filter
        If p_nTranStat >= 0 Then
            lsSQL = AddCondition(getSQ_Browse, "a.cTranStat IN (" & strDissect(p_nTranStat) & ")")
        Else
            lsSQL = getSQ_Browse()
        End If

        If p_sBranchCD <> "" Then
            lsSQL = AddCondition(lsSQL, "a.sTransNox LIKE " & strParm(p_sBranchCD & "%"))
        End If

        'create Kwiksearch filter
        Dim lsFilter As String
        If fbByCode Then
            lsFilter = "b.sCompnyNm LIKE " & strParm("%" & fsValue & "%")
        Else
            lsFilter = "a.dTransact like " & strParm(fsValue & "%")
        End If
        Debug.Print(lsSQL)
        Dim loDta As DataRow = KwikSearch(p_oApp _
                                        , lsSQL _
                                        , False _
                                        , lsFilter _
                                        , "sTransNox»sCompnyNm»dTransact»cTranStat" _
                                        , "Trans No»Client»Date»Status",
                                        , "a.sTransNox»b.sCompnyNm»a.dTransact»a.cTranStat" _
                                        , IIf(fbByCode, 0, 1))
        If IsNothing(loDta) Then
            p_nEditMode = xeEditMode.MODE_UNKNOWN
            Return False
        Else
            Return OpenTransaction(loDta.Item("sTransNox"))
        End If
    End Function

    'Public Function SaveTransaction
    'This object does not implement Update
    Public Function SaveTransaction() As Boolean
        If Not (p_nEditMode = xeEditMode.MODE_ADDNEW Or
                p_nEditMode = xeEditMode.MODE_READY Or
                p_nEditMode = xeEditMode.MODE_UPDATE) Then
            MsgBox("Invalid Edit Mode detected!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, p_sMsgHeadr)
            Return False
        End If

        If Not isEntryOk() Then
            Return False
        End If

        If MsgBox("Do you want to Save this Transaction?", vbQuestion + vbYesNo, "Confirm") = vbNo Then Return False

        Dim lsSQL As String
        Dim lnTotal As Double
        Dim lnRow As Integer
        Dim lnEntryNox As Integer


        If p_nEditMode = xeEditMode.MODE_ADDNEW Then

            p_oDTMaster(0).Item("sTransNox") = GetNextCode(p_sMasTable, "sTransNox", True, p_oApp.Connection, True, p_sBranchCD)
            p_oDTMaster(0).Item("dModified") = Format(p_oApp.SysDate, "yyyy-MM-dd HH:mm:ss")

            p_oApp.BeginTransaction()
            lnEntryNox = 0
            lnTotal = 0
            'inserting detailsTransNox
            For lnCtr = 0 To p_oBillDetail.Rows.Count - 1
                Dim cCollectdValue = p_oBillDetail.Rows(lnCtr)("cBilledxx")
                If Not (String.IsNullOrEmpty(cCollectdValue.ToString()) OrElse cCollectdValue = 0) Then
                    lnTotal += CDbl(p_oBillDetail.Rows(lnCtr)("nAmountxx"))
                    Debug.Print(strParm(p_oBillDetail.Rows(lnCtr)("sTransNox")))
                    lsSQL = "INSERT INTO " & p_sDetTable & " SET" &
                                   "  sTransNox = " & strParm(p_oDTMaster.Rows(0)("sTransNox")) &
                                   ", nEntryNox = " & CDbl(lnEntryNox + 1) &
                                   ", sSourceNo = " & strParm(p_oBillDetail.Rows(lnCtr)("sTransNox")) &
                                   ", nAmountxx = " & CDbl(p_oBillDetail.Rows(lnCtr)("nAmountxx")) &
                                   ", cPaidxxxx = " & xeLogical.NO

                    Try

                        lnRow = p_oApp.Execute(lsSQL, p_sDetTable)
                        If lnRow <= 0 Then
                            MsgBox("Unable to Save Transaction!!!" & vbCrLf &
                                    "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                            Return False
                        End If
                        'update billing detail
                        lsSQL = "UPDATE Delivery_Service_Trans SET" &
                            "  cBilledxx = " & xeLogical.YES &
                            ",  dBilledxx = " & datetimeParm(p_oApp.SysDate) &
                        " WHERE sTransNox = " & strParm(p_oBillDetail.Rows(lnCtr)("sTransNox"))
                        Try
                            lnRow = p_oApp.Execute(lsSQL, "Delivery_Service_Trans")
                            If lnRow <= 0 Then
                                MsgBox("Unable to Save Transaction!!!" & vbCrLf &
"Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                                Return False
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try
                    Catch ex As Exception
                        Throw ex
                    End Try
                    lnEntryNox = lnEntryNox + 1
                End If
            Next

            'inserting master


            p_oDTMaster(0).Item("nEntryNox") = CDbl(lnEntryNox)
            p_oDTMaster(0).Item("nTranTotl") = CDbl(lnTotal)

            lsSQL = ADO2SQL(p_oDTMaster, p_sMasTable, , p_oApp.UserID)
            If (p_oApp.Execute(lsSQL, p_sMasTable) <= 0) Then
                p_oApp.RollBackTransaction()
                Return False
            End If

            'inserting of client if not exist
            If p_oDTMaster(0).Item("sClientID") <> "" Then
                If Not p_oClient.SaveClient Then
                    MsgBox("Unable to save client info!", vbOKOnly, p_sMsgHeadr)
                    If p_sBranchCD = "" Then p_oApp.RollBackTransaction()
                    Return False
                End If

                p_oDTMaster(0).Item("sClientID") = p_oClient.Master("sClientID")
            End If
            p_oApp.CommitTransaction()
        Else
            lsSQL = ADO2SQL(p_oDTMaster, p_sMasTable, "sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")), p_oApp.UserID)

            If lsSQL <> "" Then
                p_oApp.BeginTransaction()
                If (p_oApp.Execute(lsSQL, p_sMasTable) <= 0) Then
                    p_oApp.RollBackTransaction()
                    Return False
                End If
                p_oApp.CommitTransaction()
            End If
        End If
        p_nEditMode = xeEditMode.MODE_UNKNOWN


        If MsgBox("Do you want to print this Transaction?", vbQuestion + vbYesNo, "Confirm") = vbYes Then
            If (PrintTransaction()) Then

            End If
        End If
        Return True
    End Function

    Public Sub SearchMaster(ByVal fnIndex As Integer, ByVal fsValue As String)
        Select Case fnIndex
            Case 2 ' sClientNm
                getClient(fnIndex, fnIndex, fsValue, False, True)

        End Select
    End Sub

    'This method implements a search master where id and desc are not joined.
    Private Sub getClient(ByVal fnColIdx As Integer _
                        , ByVal fnColDsc As Integer _
                        , ByVal fsValue As String _
                        , ByVal fbIsCode As Boolean _
                        , ByVal fbIsSrch As Boolean)

        'Compare the value to be search against the value in our column
        If fbIsCode Then
            If fsValue = p_oDTMaster(0).Item(fnColIdx) And fsValue <> "" And p_oOthersx.sClientNm <> "" Then Exit Sub
        Else
            If fsValue = p_oOthersx.sClientNm And fsValue <> "" Then Exit Sub
        End If

        Dim loClient As ggcClient.Client
        loClient = New ggcClient.Client(p_oApp)
        loClient.Parent = "LPSOA"

        'Assume that a call to this module using CLIENT ID is open
        If fbIsCode Then
            If loClient.OpenClient(fsValue) Then
                p_oClient = loClient
                p_oDTMaster(0).Item("sClientID") = p_oClient.Master("sClientID")
                p_oOthersx.sClientNm = p_oClient.Master("sLastName") & ", " &
                                       p_oClient.Master("sFrstName") &
                                       IIf(p_oClient.Master("sSuffixNm") = "", "", " " & p_oClient.Master("sSuffixNm")) & " " &
                                       p_oClient.Master("sMiddName")

                p_oOthersx.sAddressx = IIf(p_oClient.Master("sHouseNox") = "", "", p_oClient.Master("sHouseNox") & " ") &
                                           p_oClient.Master("sAddressx") & ", " &
                                           p_oClient.Master("sTownName")
            Else
                p_oDTMaster(0).Item("sClientID") = ""
                p_oOthersx.sClientNm = ""
                p_oOthersx.sAddressx = ""
            End If

            RaiseEvent MasterRetrieved(fnColDsc, p_oOthersx.sClientNm)
            RaiseEvent MasterRetrieved(80, p_oOthersx.sAddressx)
            Exit Sub
        End If

        'A call to this module using client name is search
        If loClient.SearchClient(fsValue, False) Then
            If loClient.ShowClient Then
                p_oClient = loClient
                p_oDTMaster(0).Item("sClientID") = p_oClient.Master("sClientID")
                p_oOthersx.sClientNm = p_oClient.Master("sLastName") & ", " &
                                       p_oClient.Master("sFrstName") &
                                       IIf(p_oClient.Master("sSuffixNm") = "", "", " " & p_oClient.Master("sSuffixNm")) & " " &
                                       p_oClient.Master("sMiddName")
                p_oOthersx.sAddressx = IIf(p_oClient.Master("sHouseNox") = "", "", p_oClient.Master("sHouseNox") & " ") &
                                           p_oClient.Master("sAddressx") & ", " &
                                           p_oClient.Master("sTownName")
            End If
        End If

        RaiseEvent MasterRetrieved(fnColDsc, p_oOthersx.sClientNm)
        RaiseEvent MasterRetrieved(80, p_oOthersx.sAddressx)
    End Sub

    Private Sub initMaster()
        Dim lnCtr As Integer
        For lnCtr = 0 To p_oDTMaster.Columns.Count - 1
            Select Case LCase(p_oDTMaster.Columns(lnCtr).ColumnName)
                Case "stransnox"
                    p_oDTMaster(0).Item(lnCtr) = GetNextCode(p_sMasTable, "sTransNox", True, p_oApp.Connection, True, p_oApp.BranchCode)

                Case "dtransact", "dmodified", "dtimestmp"
                    p_oDTMaster(0).Item(lnCtr) = p_oApp.SysDate

                Case "smodified"
                    p_oDTMaster(0).Item(lnCtr) = p_oApp.UserID
                Case "ctranstat"
                    p_oDTMaster(0).Item(lnCtr) = 0
                Case "nentrynox"
                    p_oDTMaster(0).Item(lnCtr) = 0
                Case "ntrantotl"
                    p_oDTMaster(0).Item(lnCtr) = 0.0
                Case "dprintedx"
                    p_oDTMaster(0).Item(lnCtr) = DBNull.Value
                Case Else
                    p_oDTMaster(0).Item(lnCtr) = ""
            End Select
        Next
    End Sub
    Private Sub InitOthers()
        p_oOthersx.sClientNm = ""
        p_oOthersx.sAddressx = ""
    End Sub

    Private Function isEntryOk() As Boolean


        If p_oDTMaster(0).Item("sSourceCd") = "" Then
            MsgBox("Source Info seems to be Empty! Please check your entry...", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
            Return False
        End If

        If p_oDTMaster(0).Item("sClientID") = "" Then
            MsgBox("Client Info seems to be Empty! Please check your entry...", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
            Return False
        End If
        If Not pbModified = True Then
            If p_nEntryCount = 0 Then
                MsgBox("Detail seems to be Empty! Please check your entry...", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
                Return False
            End If
        End If
        Return True
    End Function



    'Public Function OpenTransaction(String)
    Public Function OpenTransaction(ByVal fsTransNox As String) As Boolean
        Dim lsSQL As String

        lsSQL = AddCondition(getSQ_Master, "a.sTransNox = " & strParm(fsTransNox))
        p_oDTMaster = p_oApp.ExecuteQuery(lsSQL)

        lsSQL = AddCondition(getSQ_DeliveryService, " d.sTransNox = " & strParm(fsTransNox))
        p_oBillDetail = p_oApp.ExecuteQuery(lsSQL)


        If p_oDTMaster.Rows.Count <= 0 Then
            p_nEditMode = xeEditMode.MODE_UNKNOWN
            Return False
        End If

        Call InitOthers()

        p_nEditMode = xeEditMode.MODE_READY
        Return True
    End Function

    Function CancelTransaction() As Boolean
        Dim lsSQL As String
        Dim lnrow As Integer
        If Not (p_nEditMode = xeEditMode.MODE_READY Or
               p_nEditMode = xeEditMode.MODE_UPDATE) Then
            MsgBox("Invalid Edit Mode detected!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, p_sMsgHeadr)
            Return False
        End If

        'Test if closing is possible
        If Not (p_nEditMode = xeEditMode.MODE_READY) Then
            MsgBox("Transaction mode does not allow Disapproving of the Record!!!" & vbCrLf & vbCrLf &
                  "Please inform the SEG/SSG of Guanzon Group of Companies!!!", vbCritical, "Warning")
            GoTo endProc
        End If

        If Not p_oDTMaster(0).Item("sTransNox") = "" Then
            If Not OpenTransaction(p_oDTMaster(0).Item("sTransNox")) Then GoTo endProc
        End If

        If p_oDTMaster(0).Item("cTranStat") = 2 _
            Or p_oDTMaster(0).Item("cTranStat") = 3 _
            Or p_oDTMaster(0).Item("cTranStat") = 1 Then
            MsgBox("Modification of closed / cancelled / posted transaction is not allowed!" & vbCrLf & vbCrLf &
                     "Please verify your entry then Try Again!!!", vbCritical, "Warning")
            GoTo endProc
        End If

        If MsgBox("Do you want to Disapprove this Transaction?", vbQuestion + vbYesNo, "Confirm") = vbNo Then GoTo endProc


        p_oApp.BeginTransaction()
        For lnCtr = 0 To p_oBillDetail.Rows.Count - 1
            Try

                'update billing detail empty
                lsSQL = "UPDATE Delivery_Service_Trans SET" &
                        "  cBilledxx = " & xeLogical.NO &
                        ",  cCollectd = " & xeLogical.NO &
                        ",  cTranStat = " & xeTranStat.TRANS_OPEN &
                        ",  dBilledxx = NULL " &
                    " WHERE sTransNox = " & strParm(p_oBillDetail.Rows(lnCtr)("sTransNox"))
                Try
                    lnrow = p_oApp.Execute(lsSQL, "Delivery_Service_Trans")
                    If lnrow <= 0 Then
                        MsgBox("Unable to Disapprove Transaction!!!" & vbCrLf &
                                    "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                        Return False
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            Catch ex As Exception
                Throw ex
            End Try
        Next


        lsSQL = "UPDATE " & p_sMasTable & " SET" &
                    " cTranStat = " & strParm("3") &
                " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox"))


        If p_oApp.Execute(lsSQL, p_sMasTable, p_sBranchCD) <= 0 Then
            p_oApp.RollBackTransaction()
            MsgBox("Unable to disapprove " & p_oDTMaster(0).Item("sTransNox") & " from " & p_sMasTable & " Table." & vbCrLf &
                     "Please Inform SEG/SSG of Guanzon Group of Companies!!!", vbCritical, "Warning")
            GoTo endProc
        End If

        p_oApp.CommitTransaction()
        p_oDTMaster(0).Item(9) = 3
        RaiseEvent MasterRetrieved(9, "3")

        p_nEditMode = xeEditMode.MODE_READY

        Return True
endProc:
        Exit Function

    End Function

    Function CloseTransaction() As Boolean
        Dim lsSQL As String
        Dim lnrow As Integer
        If Not (p_nEditMode = xeEditMode.MODE_READY Or
               p_nEditMode = xeEditMode.MODE_UPDATE) Then
            MsgBox("Invalid Edit Mode detected!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, p_sMsgHeadr)
            Return False
        End If



        If Not p_oDTMaster(0).Item("sTransNox") = "" Then
            If Not OpenTransaction(p_oDTMaster(0).Item("sTransNox")) Then GoTo endProc
        End If

        If p_oDTMaster(0).Item("cTranStat") = 2 _
            Or p_oDTMaster(0).Item("cTranStat") = 3 _
            Or p_oDTMaster(0).Item("cTranStat") = 1 Then
            MsgBox("Modification of closed / cancelled / posted transaction is not allowed!" & vbCrLf & vbCrLf &
                     "Please verify your entry then Try Again!!!", vbCritical, "Warning")
            GoTo endProc
        End If

        If MsgBox("Do you want to Approve this Transaction?", vbQuestion + vbYesNo, "Confirm") = vbNo Then GoTo endProc

        p_oApp.BeginTransaction()

        lsSQL = "UPDATE " & p_sMasTable & " SET" &
                    " cTranStat = " & strParm("1") &
                        ", sModified = " & strParm(p_oApp.UserID) &
                        ", dModified = " & dateParm(p_oApp.SysDate) &
                " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox"))


        If p_oApp.Execute(lsSQL, p_sMasTable, p_sBranchCD) <= 0 Then
            p_oApp.RollBackTransaction()
            MsgBox("Unable to approve " & p_oDTMaster(0).Item("sTransNox") & " from " & p_sMasTable & " Table." & vbCrLf &
                     "Please Inform SEG/SSG of Guanzon Group of Companies!!!", vbCritical, "Warning")
            GoTo endProc
        End If

        p_oApp.CommitTransaction()
        p_oDTMaster(0).Item(9) = 1
        RaiseEvent MasterRetrieved(9, "1")

        p_nEditMode = xeEditMode.MODE_READY

        If MsgBox("Do you want to print this Transaction?", vbQuestion + vbYesNo, "Confirm") = vbYes Then
            If (PrintTransaction()) Then

            End If
        End If
        Return True
endProc:
        Exit Function

    End Function

    Function PostTransaction() As Boolean
        Dim lsSQL As String
        If Not (p_nEditMode = xeEditMode.MODE_READY Or
               p_nEditMode = xeEditMode.MODE_UPDATE) Then
            MsgBox("Invalid Edit Mode detected!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, p_sMsgHeadr)
            Return False
        End If

        If Not p_oDTMaster(0).Item("sTransNox") = "" Then
            If Not OpenTransaction(p_oDTMaster(0).Item("sTransNox")) Then GoTo endProc
        End If

        p_oApp.BeginTransaction()

        lsSQL = "UPDATE " & p_sMasTable & " SET" &
                    " cTranStat = " & strParm("2") &
                        ", sModified = " & strParm(p_oApp.UserID) &
                        ", dModified = " & dateParm(p_oApp.SysDate) &
                " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox"))

        If p_oApp.Execute(lsSQL, p_sMasTable, p_sBranchCD) <= 0 Then
            p_oApp.RollBackTransaction()
            MsgBox("Unable to approve " & p_oDTMaster(0).Item("sTransNox") & " from " & p_sMasTable & " Table." & vbCrLf &
                     "Please Inform SEG/SSG of Guanzon Group of Companies!!!", vbCritical, "Warning")
            GoTo endProc
        End If

        p_oApp.CommitTransaction()
        p_oDTMaster(0).Item(9) = 2
        RaiseEvent MasterRetrieved(9, "2")

        p_nEditMode = xeEditMode.MODE_READY

        MsgBox("This Transaction is Now Fully Paid .", MsgBoxStyle.Information, "Notice")
        Return True
endProc:
        Return False
        Exit Function

    End Function


    'Public Function PrePostTransaction
    'This object does not implement Update
    Public Function PrePostTransaction() As Boolean
        If Not (p_nEditMode = xeEditMode.MODE_ADDNEW Or
                p_nEditMode = xeEditMode.MODE_READY Or
                p_nEditMode = xeEditMode.MODE_UPDATE) Then
            MsgBox("Invalid Edit Mode detected!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, p_sMsgHeadr)
            Return False
        End If

        If Not isEntryOk() Then
            Return False
        End If

        If MsgBox("Do you want to Tag this selected Detail as paid?", vbQuestion + vbYesNo, "Confirm") = vbNo Then Return False

        Dim lsSQL As String
        Dim lnRow As Integer


        p_oApp.BeginTransaction()

        'inserting detail
        For lnCtr = 0 To p_oBillDetail.Rows.Count - 1
            If (p_oBillDetail.Rows(lnCtr)("cPaidxxxx") = 1) Then
                lsSQL = "UPDATE " & p_sDetTable & " SET " &
                            "  cPaidxxxx = " & xeLogical.YES &
                " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                        " AND sSourceNo = " & strParm(p_oBillDetail.Rows(lnCtr)("sTransNox"))

                Try

                    lnRow = p_oApp.Execute(lsSQL, p_sDetTable)
                    If lnRow <= 0 Then
                        MsgBox("Unable to Tag as Pay the Detail!!!" & vbCrLf &
                                    "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                        Return False
                    End If
                    'update billing detail
                    lsSQL = "UPDATE Delivery_Service_Trans SET " &
                            " cCollectd = " & xeLogical.YES &
                            ", cPaidxxxx = " & xeLogical.YES &
                            ", dPaidxxxx = " & datetimeParm(p_oApp.SysDate) &
                            ", cTranStat = " & xeTranStat.TRANS_POSTED &
                            ", dModified = " & datetimeParm(p_oApp.SysDate) &
                        " WHERE sTransNox = " & strParm(p_oBillDetail.Rows(lnCtr)("sTransNox"))
                    Try
                        lnRow = p_oApp.Execute(lsSQL, "Delivery_Service_Trans")
                        If lnRow <= 0 Then
                            MsgBox("Unable to Tag as Pay the Detail !!!" & vbCrLf &
                                        "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                            Return False
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
                Catch ex As Exception
                    Throw ex
                End Try
            End If
        Next
        p_oApp.CommitTransaction()


        If isAllPaid() Then
            If (PostTransaction()) Then
            End If
        Else
            If Not p_oDTMaster(0).Item("sTransNox") = "" Then
                If Not OpenTransaction(p_oDTMaster(0).Item("sTransNox")) Then Return False
            End If
        End If

        Return True

    End Function

    Private Function isAllPaid()

        Dim lnRow As Integer
        For lnCtr = 0 To p_oBillDetail.Rows.Count - 1
            If Not (p_oBillDetail.Rows(lnCtr)("cPaidxxxx") = 1) Then
                Return False
            End If
        Next
        Return True
    End Function
    'Private Function getSQ_Detail() As String

    '    Return "SELECT a.sTransNox sTransNox " &
    '                ", a.nEntryNox nEntryNox " &
    '                ", a.sSourceNo sSourceNo " &
    '                ", a.nAmountxx nAmountxx " &
    '                ", a.cPaidxxxx cPaidxxxx " &
    '                ", a.dTimeStmp dTimeStmp " &
    '            " FROM " & p_sDetTable & " a "
    'End Function
    Private Function getSQ_Master() As String
        Return "SELECT a.sTransNox sTransNox" &
                    ", a.dTransact dTransact" &
                    ", a.sClientID sClientID" &
                    ", a.sSourceCd sSourceCd" &
                    ", a.nEntryNox nEntryNox" &
                    ", a.nTranTotl nTranTotl" &
                    ", a.sRemarksx sRemarksx" &
                    ", a.cPrintedx cPrintedx" &
                    ", a.dPrintedx dPrintedx" &
                    ", a.cTranStat cTranStat" &
                    ", a.sModified sModified" &
                    ", a.dModified dModified" &
                    ", a.dTimeStmp dTimeStmp" &
              " FROM " & p_sMasTable & " a" &
                    ", Client_Master b" &
              " WHERE a.sClientID = b.sClientID"
    End Function

    Private Function getSQ_Browse() As String
        Return "SELECT a.sTransNox sTransNox" &
                    ", b.sCompnyNm sCompnyNm" &
                    ", a.dTransact dTransact" &
                    ", a.sSourceCd sSourceCd" &
                    ", CASE " &
                    " WHEN a.cTranStat = '0' THEN 'OPEN' " &
                    " WHEN a.cTranStat = '1' THEN 'APPROVED'" &
                    " WHEN a.cTranStat = '2' THEN 'FULLY PAID'" &
                    " WHEN a.cTranStat = '3' THEN 'DISAPPROVED'" &
              " End As cTranStat " &
              " FROM " & p_sMasTable & " a" &
                    ", Client_Master b" &
              " WHERE a.sClientID = b.sClientID " &
              " ORDER BY a.dTransact,a.sTransNox DESC "
    End Function

    Private Function getSQ_DeliveryService() As String

        Return "SELECT a.sTransNox" &
                  ", b.sDescript" &
                  ", a.sSourceNo" &
                  ", c.dTransact" &
                  ", a.nAmountxx" &
                  " ,a.cBilledxx" &
                  ", a.sRemarksx" &
                  ", a.sSourceCd" &
                  ", a.cCollectd" &
                  ", a.dBilledxx" &
                  ", d.cPaidxxxx" &
                  ", a.dPaidxxxx" &
                  ", a.cWaivexxx" &
                  ", a.cTranStat" &
                  ", b.sBriefDsc" &
                  ", a.sRiderIDx" &
              " FROM Delivery_Service_Trans a " &
              " LEFT JOIN Delivery_Service b " &
              " ON a.sRiderIDx = b.sRiderIDx " &
              " LEFT JOIN SO_Master c " &
              " ON a.sSourceNo = c.sTransNox " &
              " LEFT JOIN Billing_Detail d " &
              " ON a.sTransNox = d.sSourceNo "

    End Function

    Public Sub New(ByVal foRider As GRider, Optional ByVal fnStatus As Integer = 0)
        p_oApp = foRider
        p_oClient = New Client(foRider)
        p_oSTRept = Nothing
        p_oReport = Nothing
        p_oBillDetail = Nothing
        p_oDTMaster = Nothing
        p_oFormxx = New frmReportViewer
        p_nEditMode = xeEditMode.MODE_UNKNOWN

        p_sBranchCD = p_oApp.BranchCode
        p_nTranStat = fnStatus
    End Sub


    Private Class Others
        Public sClientNm As String
        Public sAddressx As String
    End Class



    Public Function loadBilling() As Boolean
        Dim fsSourceCd As String
        Dim lsSQL As String
        '    Dim lsMergeID As String
        '    Dim loDT As DataTable
        '    Dim loReturn As DataTable
        lsSQL = ""
        fsSourceCd = p_oDTMaster.Rows(0)("sSourceCd")
        If fsSourceCd = "DS" Then
            lsSQL = AddCondition(getSQ_DeliveryService, "a.cBilledxx <> " & xeRecordStat.RECORD_NEW & " GROUP BY a.sSourceNo ")
        Else
            lsSQL = ""
        End If

        'createDetailTable()
        If lsSQL <> "" Then
            p_oBillDetail = p_oApp.ExecuteQuery(lsSQL)
        Else
            p_oBillDetail = Nothing
            MsgBox("Its seems there is no Transaction to Bill ! Please double-check your entry...", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
            Return False

        End If

        If p_oBillDetail.Rows.Count = 0 Then
            p_oBillDetail = Nothing
            MsgBox("Its seems there is no Transaction to Bill ! Please double-check your entry...", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
            Return False
        End If



        Return True
    End Function

#Region "Printing Region"

    Public Function PrintTransaction() As Boolean
        Dim oProg As frmProgress
        Dim lnRow As Integer
        Dim lsSQL As String 'whole statement
        Dim lsQuery1 As String
        p_oReport = New ReportDocument

        'check if cancel or fully paid
        If p_oDTMaster(0).Item("cTranStat") = 2 _
            Or p_oDTMaster(0).Item("cTranStat") = 3 Then
            MsgBox("Printing of Disapproved / Fully Paid / Posted / Cancel transaction is not allowed!" & vbCrLf & vbCrLf &
                     "Please verify your entry then Try Again!!!", vbCritical, "Warning")
            Return False
        End If

        'Show progress bar
        oProg = New frmProgress
        oProg.PistonInfo = p_oApp.AppPath & "/piston.avi"
        oProg.ShowTitle("EXTRACTING RECORDS FROM DATABASE")
        oProg.ShowProcess("Please wait...")
        oProg.Show()


        If lsSQL <> "" Then
            lsSQL = lsSQL & " ORDER BY sAgentIDx, dModified ASC"
        End If
        Debug.Print(lsSQL)
        If Not (OpenTransaction(p_oDTMaster(0).Item("sTransNox"))) Then
            MsgBox("Unable to Print Transaction!!!" & vbCrLf &
                                "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
            Return False
        End If

        Dim loDtaTbl As DataTable = getRptTable()
        Dim lnCtr As Integer

        If p_oBillDetail Is Nothing OrElse p_oBillDetail.Rows.Count = 0 Then
            oProg.Close()
            Return False
        End If
        oProg.ShowTitle("LOADING RECORDS")

        oProg.MaxValue = p_oBillDetail.Rows.Count

        For lnCtr = 0 To p_oBillDetail.Rows.Count - 1

            oProg.ShowProcess("Loading " & p_oBillDetail(lnCtr).Item("sTransNox") & "...")

            loDtaTbl.Rows.Add(addRow(lnCtr, loDtaTbl))
        Next

        oProg.ShowSuccess()

        p_oReport.Load(p_oApp.AppPath & "\vb.net\RetMgtSys\Reports\SOAOfficial.rpt")

        Dim loRpt As ReportDocument = p_oReport

        Dim loTxtObj As CrystalDecisions.CrystalReports.Engine.TextObject
        'loTxtObj = loRpt.ReportDefinition.Sections(0).ReportObjects("txtCompany")
        'loTxtObj.Text = p_oApp.BranchName

        ''Set Branch Address
        'loTxtObj = loRpt.ReportDefinition.Sections(0).ReportObjects("txtAddress")
        'loTxtObj.Text = p_oApp.Address & vbCrLf & p_oApp.TownCity & " " & p_oApp.ZippCode & vbCrLf & p_oApp.Province
        'Set First Header
        loTxtObj = loRpt.ReportDefinition.Sections(1).ReportObjects("txtHeading1")
        loTxtObj.Text = "STATEMENT OF ACCOUNT"

        'Set Second Header
        loTxtObj = loRpt.ReportDefinition.Sections(1).ReportObjects("txtDate")
        loTxtObj.Text = Format(p_oApp.SysDate, "MMMM dd, yyyy")
        'Set 3rd Header
        loTxtObj = loRpt.ReportDefinition.Sections(3).ReportObjects("txtTransNo")
        loTxtObj.Text = p_oDTMaster(0).Item("sTransNox")

        loTxtObj = loRpt.ReportDefinition.Sections(3).ReportObjects("txtRptUser")
        loTxtObj.Text = Decrypt(p_oApp.UserName, "08220326")

        'Set masterdetails
        loTxtObj = loRpt.ReportDefinition.Sections(4).ReportObjects("txtTotalAmount")
        loTxtObj.Text = FormatNumber(p_oDTMaster(0).Item("nTranTotl"), 2)


        'setting client
        If Trim(IFNull(p_oDTMaster(0).Item(2))) <> "" And Trim(p_oOthersx.sClientNm) = "" Then
            getClient(2, 2, p_oDTMaster(0).Item(2), True, False)
        End If
        loTxtObj = loRpt.ReportDefinition.Sections(1).ReportObjects("txtClientNme")
        loTxtObj.Text = p_oOthersx.sClientNm
        loTxtObj = loRpt.ReportDefinition.Sections(1).ReportObjects("txtClientAddress")
        loTxtObj.Text = p_oOthersx.sAddressx

        If p_oDTMaster(0).Item("cTranStat") > 0 Then
            loRpt.ReportDefinition.Sections(1).ReportObjects("bgdraft").ObjectFormat.EnableSuppress = True
        End If
        loRpt.SetDataSource(p_oSTRept)

        p_oFormxx.ReportDocument = p_oReport
        p_oFormxx.ShowDialog()

        'updating status 
        'master table

        If p_oDTMaster(0).Item("cTranStat") > 0 Then
            lsSQL = "UPDATE " & p_sMasTable & " SET " &
                        "  cPrintedx = " & xeLogical.YES &
                        ",  dPrintedx = " & datetimeParm(p_oApp.SysDate) &
                        ", sModified = " & strParm(p_oApp.UserID) &
                        ", dModified = " & datetimeParm(p_oApp.SysDate) &
                    " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox"))

            Try
                lnRow = p_oApp.Execute(lsSQL, p_sMasTable)
                If lnRow <= 0 Then
                    MsgBox("Unable to Print Transaction!!!" & vbCrLf &
                                        "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                    Return False
                End If
            Catch ex As Exception
                Throw ex
            End Try

            'detail table
            For lnCtr = 0 To p_oBillDetail.Rows.Count - 1

                'update billing detail
                If Not p_oBillDetail(lnCtr).Item("cTranStat") = 2 Then
                    lsSQL = "UPDATE Delivery_Service_Trans SET" &
                        " dBilledxx = " & datetimeParm(p_oApp.SysDate) &
                        ", cTranStat = " & xeAccountStat.CLOSED &
                    " WHERE sTransNox = " & strParm(p_oBillDetail.Rows(lnCtr)("sTransNox"))
                    Try
                        lnRow = p_oApp.Execute(lsSQL, "Delivery_Service_Trans")
                        If lnRow <= 0 Then
                            MsgBox("Unable to Print Transaction!!!" & vbCrLf &
                                "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                            Return False
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try

                End If
            Next
        End If
        Return True
    End Function
    Private Function getRptTable() As DataTable
        'Initialize DataSet
        p_oSTRept = New DataSet

        'Load the data structure of the Dataset
        'Data structure was saved at DataSet1.xsd 
        p_oSTRept.ReadXmlSchema(p_oApp.AppPath & "\vb.net\RetMgtSys\Reports\DataSet1.xsd")

        'Return the schema of the datatable derive from the DataSet 
        Return p_oSTRept.Tables(0)
    End Function

    Private Function addRow(ByVal lnRow As Integer, ByVal foSchemaTable As DataTable) As DataRow
        'ByVal foDTInclue As DataTable
        Dim loDtaRow As DataRow

        'Create row based on the schema of foSchemaTable
        loDtaRow = foSchemaTable.NewRow

        loDtaRow.Item("nField01") = lnRow + 1
        loDtaRow.Item("sField01") = p_oBillDetail(lnRow).Item("sSourceNo")
        loDtaRow.Item("sField02") = Format(p_oBillDetail(lnRow).Item("dTransact"), "MMMM dd, yyyy")
        loDtaRow.Item("lField01") = p_oBillDetail(lnRow).Item("nAmountxx")
        Return loDtaRow
    End Function
#End Region
    Function TranStatus(ByVal fnStatus As Int32) As String
        If fnStatus = 0 Then
            Return "OPEN"
        ElseIf fnStatus = 1 Then
            Return "APPROVED"
        ElseIf fnStatus = 2 Then
            Return "FULLY PAID"
        ElseIf fnStatus = 3 Then
            Return "DISAPPROVED"
        ElseIf fnStatus = 4 Then
            Return "VOID"
        Else
            Return "UNKNOWN"
        End If
    End Function
End Class
