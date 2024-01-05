
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

Public Class clsSOA
    Private p_oApp As GRider
    Private p_oDTMaster As DataTable
    Private p_oDTDetail As DataTable
    Private p_oBillDetail As DataTable


    Private p_oClient As ggcClient.Client
    Private p_nEditMode As xeEditMode
    Private p_oOthersx As New Others
    Private p_oSOADetail As New clsSOAInfo
    Private p_nTranStat As Int32  'Transaction Status of the transaction to retrieve
    Private p_sBranchCD As String


    Private Const p_sMasTable As String = "Billing_Master"
    Private Const p_sDetTable As String = "Billing_Detail"
    Private Const p_sMsgHeadr As String = "Statement of Account's"

    Public Event MasterRetrieved(ByVal Index As Integer,
                                  ByVal Value As Object)
    Public Event DetailRetrieved(ByVal Row As Integer, ByVal Index As Integer,
                              ByVal Value As Object)

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


            End If
        End Set
    End Property

    'Property Master(String)
    Public Property Master(ByVal Index As String) As Object
        Get
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index
                    'Case "sclientnm" ' 

                    '    Return p_oOthersx.sClientNm
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

                    Case Else
                        p_oDTMaster(0).Item(Index) = value
                End Select
            End If
        End Set
    End Property
    Public Property Detail(ByVal Row As Integer, ByVal Index As Integer) As Object
        Get
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index

                    'Case 2 ' sClientNm

                    'Case 3 ' sMobileNo


                    Case Else
                        Return p_oDTDetail(0).Item(Index)
                End Select
            Else
                Return vbEmpty
            End If
        End Get

        Set(ByVal value As Object)
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index
                    Case 8
                        If IsDate(value) Then
                            p_oDTMaster(0).Item(Index) = Format(CDate(value), "yyyy-MM-dd")

                        End If


                        RaiseEvent DetailRetrieved(Row, Index, p_oDTDetail(0).Item(Index))

                        p_oDTMaster(0).Item(Index) = value
                End Select
            End If
        End Set
    End Property

    'Property Detail(String)
    Public Property Detail(ByVal Row As Integer, ByVal Index As String) As Object
        Get
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case LCase(Index)

                    Case Else
                        Return p_oDTDetail(0).Item(Index)
                End Select
            Else
                Return vbEmpty
            End If
        End Get
        Set(ByVal value As Object)
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case LCase(Index)

                    Case "sremarksx" '9
                        If (value <> "") Then
                            p_oDTMaster(0).Item(Index) = value
                        End If

                        RaiseEvent DetailRetrieved(Row, Index, p_oDTDetail(0).Item(Index))
                End Select
            End If
        End Set
    End Property
    Public ReadOnly Property BillDetail(ByVal Row As Integer, ByVal Index As Integer) As Object
        Get
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index

                    'Case 2 ' sClientNm
                    '    If Trim(IFNull(p_oDTMaster(0).Item(2))) <> "" And Trim(p_oSOADetail.xClientNmeCallInfo) = "" Then
                    '        'getClients(2, 2, p_oDTMaster(0).Item(2), True, False)
                    '    End If
                    '    Return p_oSOADetail.xClientNmeCallInfo
                    'Case 3 ' sMobileNo
                    '    Return p_oDTMaster(0).Item(2)

                    Case Else
                        Return p_oBillDetail(0).Item(Index)
                End Select
            Else
                Return vbEmpty
            End If
        End Get


    End Property

    'Property Detail(String)
    Public ReadOnly Property BillDetail(ByVal Row As Integer, ByVal Index As String) As Object
        Get
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case LCase(Index)

                    Case Else
                        Return p_oBillDetail(0).Item(Index)
                End Select
            Else
                Return vbEmpty
            End If
        End Get

    End Property
    'Property EditMode()
    Public ReadOnly Property EditMode() As xeEditMode
        Get
            Return p_nEditMode
        End Get
    End Property

    Public Function GetItemCount() As Integer
        If p_oDTDetail Is Nothing Then Return 0

        Return p_oDTDetail.Rows.Count
    End Function

    Public Function GetItemDSCount() As Integer
        If p_oBillDetail Is Nothing Then Return 0

        Return p_oBillDetail.Rows.Count
    End Function

    'Public Function NewTransaction()
    Public Function NewTransaction() As Boolean
        Dim lsSQL As String

        lsSQL = AddCondition(getSQ_Master, "0=1")
        p_oDTMaster = p_oApp.ExecuteQuery(lsSQL)
        p_oDTMaster.Rows.Add(p_oDTMaster.NewRow())

        p_oDTDetail = p_oApp.ExecuteQuery(getSQ_Detail)
        'p_oDTDetail.Rows.Add(p_oDTDetail.NewRow())
        Call initMaster()
        Call InitOthers()
        Call InitSOAInfo()

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
                                        , "sTransNox»sCompnyNm»dTransact" _
                                        , "Trans No»Client»Date",
                                        , "a.sTransNox»b.sCompnyNm»a.dTransact" _
                                        , IIf(fbByCode, 1, 2))
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

        Dim lsSQL As String


        If p_nEditMode = xeEditMode.MODE_ADDNEW Then

            p_oDTMaster(0).Item("sTransNox") = GetNextCode(p_sMasTable, "sTransNox", True, p_oApp.Connection, True, p_sBranchCD)
            p_oDTMaster(0).Item("dModified") = Format(p_oApp.SysDate, "yyyy-MM-dd HH:mm:ss")

            lsSQL = ADO2SQL(p_oDTMaster, p_sMasTable, , p_oApp.UserID)
            p_oApp.BeginTransaction()
            If (p_oApp.Execute(lsSQL, p_sMasTable) <= 0) Then
                p_oApp.RollBackTransaction()
                Return False
            End If
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
    Private Sub InitSOAInfo()
        p_oSOADetail.sTransNox = ""
        p_oSOADetail.nEntryNox = ""
        p_oSOADetail.sSourceNo = ""
        p_oSOADetail.nAmountxx = ""
        p_oSOADetail.cPaidxxxx = ""
        p_oSOADetail.dTimeStmp = ""
        p_oSOADetail.cBilledxx = ""
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

        'If p_oDTDetail.Rows.Count = 0 Then
        '    MsgBox("Detail seems to be Empty! Please check your entry...", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
        '    Return False
        'End If
        Return True
    End Function



    'Public Function OpenTransaction(String)
    Public Function OpenTransaction(ByVal fsTransNox As String) As Boolean
        Dim lsSQL As String

        lsSQL = AddCondition(getSQ_Master, "a.sTransNox = " & strParm(fsTransNox))
        p_oDTMaster = p_oApp.ExecuteQuery(lsSQL)

        If p_oDTMaster.Rows.Count <= 0 Then
            p_nEditMode = xeEditMode.MODE_UNKNOWN
            Return False
        End If

        Call InitOthers()

        p_nEditMode = xeEditMode.MODE_READY
        Return True
    End Function

    Public Function AddDetail(ByVal lnRow As Integer) As Boolean
        Try
            Dim newDetailRow As DataRow = p_oDTDetail.Rows.Add()

            With p_oBillDetail.Rows(lnRow)
                newDetailRow("sTransNox") = p_oDTMaster.Rows(0)("sTransNox")
                newDetailRow("nEntryNox") = p_oDTDetail.Rows.Count
                newDetailRow("sSourceNo") = .Item("sTransNox")
                newDetailRow("nAmountxx") = .Item("nAmountxx")
                newDetailRow("cPaidxxxx") = 0
            End With

            Return True
        Catch ex As Exception
            MsgBox("Error Exception :" + ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
            Return False
        End Try
    End Function

    Public Function RemoveDetail(ByVal rowIndex As Integer) As Boolean
        Try
            If rowIndex >= 0 AndAlso rowIndex < p_oDTDetail.Rows.Count Then
                ' Remove the row at the specified index
                p_oDTDetail.Rows.RemoveAt(rowIndex)
                Return True
            Else
                MsgBox("No Detail to remove! Please check your entry...", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
                Return False
            End If
        Catch ex As Exception
            MsgBox("Error Exception :" + ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
            Return False
        End Try
    End Function

    Private Function getSQ_Detail() As String

        Return "SELECT a.sTransNox" &
                    ", a.nEntryNox" &
                    ", a.sSourceNo" &
                    ", a.nAmountxx" &
                    ", a.cPaidxxxx" &
                    ", a.dTimeStmp" &
                " FROM " & p_sDetTable & " a"
    End Function
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
              " FROM " & p_sMasTable & " a" &
                    ", Client_Master b" &
              " WHERE a.sClientID = b.sClientID"
    End Function

    Private Function getSQ_DeliveryService() As String

        Return "SELECT a.sTransNox" &
                  ", a.sRiderIDx" &
                  ", a.sRemarksx" &
                  ", a.nAmountxx" &
                  ", a.sSourceCd" &
                  ", a.sSourceNo" &
                  ", a.cCollectd" &
                  ", a.cBilledxx" &
                  ", a.dBilledxx" &
                  ", a.cPaidxxxx" &
                  ", a.dPaidxxxx" &
                  ", a.cWaivexxx" &
                  ", a.cTranStat" &
                  ", b.sBriefDsc" &
                  ", a.sDescript" &
              " FROM Delivery_Service_Trans a " &
              " , Delivery_Service b" &
              " , Billing_Detail c" &
              " WHERE a.sRiderIDx = b.sRiderIDx" &
              " a.sTransNox = c.sSourceNo"

    End Function

    Public Sub New(ByVal foRider As GRider)
        p_oApp = foRider
        p_oClient = New Client(foRider)
        p_nEditMode = xeEditMode.MODE_UNKNOWN

        p_sBranchCD = p_oApp.BranchCode
    End Sub

    Public Sub New(ByVal foRider As GRider, ByVal fnStatus As Int32)
        Me.New(foRider)
        p_nTranStat = fnStatus
    End Sub
    Private Class Others
        Public sClientNm As String
        Public sAddressx As String
    End Class

    Private Class clsSOAInfo
        Public sTransNox As String
        Public nEntryNox As String
        Public sSourceNo As String
        Public nAmountxx As String
        Public cPaidxxxx As String
        Public dTimeStmp As String
        Public cBilledxx As String


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
            lsSQL = getSQ_DeliveryService()
        Else
            lsSQL = getSQ_DeliveryService()
        End If

        'createDetailTable()
        p_oBillDetail = p_oApp.ExecuteQuery(lsSQL)

        Debug.Print(p_oBillDetail.Rows.Count)
        If p_oBillDetail.Rows.Count = 0 Then Return False
        'With p_oBillDetail

        '    For lnCtr = 0 To p_oBillDetail.Rows.Count - 1
        '        p_oDTDetail.Rows.Add()
        '        p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("sTransNox") = p_oDTMaster.Rows(0)("sTransNox")
        '        p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("nEntryNox") = lnCtr + 1
        '        p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("sSourceNo") = .Rows(lnCtr)("sTransNox")
        '        p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("nAmountxx") = .Rows(lnCtr)("nAmountxx")
        '        p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("cPaidxxxx") = 0

        '    Next lnCtr
        'End With


        Return True
    End Function


End Class
