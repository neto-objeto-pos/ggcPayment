Imports ggcAppDriver
Imports ggcRetailParams
Imports MySql.Data.MySqlClient

Public Class Delivery

#Region "Constant"
    Private Const pxeMODULENAME As String = "Delivery"
    Private Const pxeMasterTble As String = "Delivery_Service_Trans"
#End Region

#Region "Protected Members"
    Protected p_oAppDrvr As GRider
    Protected p_oDelivery As clsDeliveryServiceParam
    Private p_oSC As New MySqlCommand
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
    Public Event MasterRetrieved(ByVal Row As Integer,
                                 ByVal Index As Object,
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

    ReadOnly Property Delivery() As DataTable
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

    Property Master(ByVal Row As Integer,
                    ByVal Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then Index = LCase(Index)
            Select Case Index
                Case "stransnox" : Index = 0
                Case "srideridx" : Index = 1
                Case "sremarksx" : Index = 2
                Case "namountxx" : Index = 3
                Case "ssourcecd" : Index = 4
                Case "ssourceno" : Index = 5
                Case "ccollectd" : Index = 6
                Case "cbilledxx" : Index = 7
                Case "dbilledxx" : Index = 8
                Case "cpaidxxxx" : Index = 9
                Case "dpaidxxxx" : Index = 10
                Case "cwaivexxx" : Index = 11
                Case "dwaivexxx" : Index = 12
                Case "swaivexxx" : Index = 13
                Case "ctranstat" : Index = 14
                Case "sbriefdsc" : Index = 15
                    ''Load the company name to the virtual field if virtual field is empty but scompnycd has value...
                    'If Trim(p_oDataTable(Row)("scompnyNm")) = "" And p_oDataTable(Row)("sCompnyCd") <> "" Then
                    '    Dim loRow As DataTable
                    '    loRow = p_oDelivery.GetAffiliate(p_oDataTable(Row)("sCompnyCd"), True)
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
            If Not IsNumeric(Index) Then Index = LCase(Index)
            Select Case Index
                Case "stransnox" : Index = 0
                Case "srideridx" : Index = 1
                Case "sremarksx" : Index = 2
                Case "namountxx"
                    Index = 3
                    If IsNumeric(Value) Then
                        p_oDataTable(Row)(Index) = Value
                    End If
                    RaiseEvent MasterRetrieved(Row, 3, Value)
                Case "ssourcecd" : Index = 4
                Case "ssourceno" : Index = 5
                Case "ccollectd" : Index = 6
                Case "cbilledxx" : Index = 7
                Case "dbilledxx" : Index = 8
                Case "cpaidxxxx" : Index = 9
                Case "dpaidxxxx" : Index = 10
                Case "cwaivexxx" : Index = 11
                Case "dwaivexxx" : Index = 12
                Case "swaivexxx" : Index = 13
                Case "ctranstat" : Index = 14
                Case "sbriefdsc" : Index = 15

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
        Dim loDT As New DataTable

        With p_oDataTable
                For lnCtr = 0 To .Rows.Count - 1
                    If Not isEntryOK(lnCtr) Then Return False

                lsSQL = "INSERT INTO " & pxeMasterTble & " SET" &
                            "  sTransNox = " & strParm(.Rows(lnCtr)("sTransNox")) &
                            ", sRiderIDx = " & strParm(.Rows(lnCtr)("sRiderIDx")) &
                            ", sRemarksx = " & strParm(.Rows(lnCtr)("sRemarksx")) &
                            ", nAmountxx = " & CDec(.Rows(lnCtr)("nAmountxx")) &
                            ", sSourceCd = " & strParm(p_sSourceCd) &
                            ", sSourceNo = " & strParm(p_sSourceNo) &
                            ", cTranStat = " & strParm(.Rows(lnCtr)("cTranStat")) &
                            ", dModified = " & dateParm(p_oAppDrvr.SysDate) &
                        " ON DUPLICATE KEY UPDATE" &
                            "  sRiderIDx = " & strParm(.Rows(lnCtr)("sRiderIDx")) &
                            ", sSourceNo = " & strParm(p_sSourceNo) &
                            ", nAmountxx = " & CDec(.Rows(lnCtr)("nAmountxx")) &
                            ", sRemarksx = " & strParm(.Rows(lnCtr)("sRemarksx"))


                Debug.Print(lsSQL)

                Try
                    lnRow = p_oAppDrvr.Execute(lsSQL, pxeMasterTble)
                    If lnRow <= 0 Then
                        MsgBox("Unable to Save Transaction!!!" & vbCrLf &
                                "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                        Return False
                    End If
                Catch ex As Exception
                    Throw ex
                End Try

                lnTotal += CDec(.Rows(lnCtr)("nAmountxx"))
            Next lnCtr
            p_nDelivery = lnTotal

            If Not p_sTransNox = String.Empty Then
                If p_oAppDrvr.Execute("DELETE FROM " & pxeMasterTble &
                                                " WHERE sSourceNo = " & strParm(p_sSourceNo) &
                                                    " AND sSourceCd = " & strParm(p_sSourceCd) &
                                                    " AND sTransNox IN(" & p_sTransNox.Substring(0, p_sTransNox.Length - 1) & ")", pxeMasterTble) <= 0 Then
                    MsgBox("Unable to Save Transaction!!!" & vbCrLf &
                                    "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                    Return False
                End If
            End If
        End With

        Return True
    End Function

    Function SearchCompany(ByVal Row As Integer,
                           Optional Value As Object = "") As Boolean

        Return getCompany(Row, Value)
    End Function

    Function SearchDelivery() As DataTable

        Return p_oDelivery.GetDeliveryService
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
                Call AddDeliveryServ()
                .Rows(lnCtr)("sTransNox") = loDT.Rows(lnCtr)("sTransNox")
                .Rows(lnCtr)("sRiderIDx") = loDT.Rows(lnCtr)("sRiderIDx")
                .Rows(lnCtr)("sRemarksx") = loDT.Rows(lnCtr)("sRemarksx")
                .Rows(lnCtr)("nAmountxx") = loDT.Rows(lnCtr)("nAmountxx")
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
                .Rows(lnCtr)("sDescript") = loDT.Rows(lnCtr)("sDescript")
                .Rows(lnCtr)("sBriefDsc") = loDT.Rows(lnCtr)("sBriefDsc")
                lnTotal += loDT.Rows(lnCtr)("nAmountxx")
            Next lnCtr
            p_nDelivery = lnTotal
        End With

        Return True
    End Function

    Function OpenBySource() As Boolean
        Dim loDT As New DataTable
        Dim lsSQL As String
        Dim lnCtr As Integer
        Dim lnTotal As Decimal

        lsSQL = AddCondition(getSQL_Master, "a.sSourceNo = " & strParm(p_sSourceNo)) &
                                                " AND a.sSourceCd = " & strParm(p_sSourceCd)

        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then Return False

        Call createTable()
        With p_oDataTable
            For lnCtr = 0 To loDT.Rows.Count - 1
                .Rows.Add()
                .Rows(lnCtr)("sTransNox") = loDT.Rows(lnCtr)("sTransNox")
                .Rows(lnCtr)("sRiderIDx") = loDT.Rows(lnCtr)("sRiderIDx")
                .Rows(lnCtr)("sRemarksx") = loDT.Rows(lnCtr)("sRemarksx")
                .Rows(lnCtr)("nAmountxx") = loDT.Rows(lnCtr)("nAmountxx")
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
                .Rows(lnCtr)("sDescript") = loDT.Rows(lnCtr)("sDescript")
                .Rows(lnCtr)("sBriefDsc") = loDT.Rows(lnCtr)("sBriefDsc")
                lnTotal += loDT.Rows(lnCtr)("nAmountxx")
            Next lnCtr
            p_nDelivery = lnTotal
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

    Function AddDeliveryServ() As Boolean
        Dim lnRow As Integer = p_oDataTable.Rows.Count

        If lnRow > 0 Then
            If Not isEntryOK(lnRow - 1) Then

            End If
            Return False
        End If
        If Not SaveTransaction() Then Return False

        With p_oDataTable
            .Rows.Add()
            .Rows(lnRow)("sTransNox") = GetNextCode(pxeMasterTble, "sTransNox", True, p_oAppDrvr.Connection, True, p_sBranchCd)
            .Rows(lnRow)("sRiderIDx") = ""
            .Rows(lnRow)("sRemarksx") = ""
            .Rows(lnRow)("nAmountxx") = 0.0
            .Rows(lnRow)("sSourceCd") = ""
            .Rows(lnRow)("sSourceNo") = ""
            .Rows(lnRow)("cTranStat") = Val(xeTranStat.TRANS_OPEN)
            .Rows(lnRow)("sBriefDsc") = ""
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
        Return " SELECT a.sTransNox ," &
                   " a.sRiderIDx," &
                   " a.sRemarksx," &
                   " a.nAmountxx," &
                   " a.sSourceCd," &
                   " a.sSourceNo," &
                   " a.cCollectd," &
                   " a.cBilledxx," &
                   " a.dBilledxx," &
                   " a.cPaidxxxx," &
                   " a.dPaidxxxx," &
                   " a.cWaivexxx," &
                   " a.dWaivexxx," &
                   " a.sWaivexxx," &
                   " a.cTranStat," &
                   " b.sDescript," &
                   " b.sBriefDsc" &
                " FROM " & pxeMasterTble & " a" &
                ", Delivery_Service b " &
                 " WHERE a.sRiderIDx = b.sRiderIDx"
    End Function

    Private Function getCompany(ByVal Row As Integer,
                                ByVal Value As String) As Boolean
        Dim lsCondition As String
        Dim lsProcName As String
        Dim loDataRow As DataRow

        lsProcName = "getCompany"

        lsCondition = String.Empty

        If Value <> String.Empty Then
            If Value = p_oDataTable(Row)("sRiderIDx") Then
                Return True
            End If
        End If

        loDataRow = p_oDelivery.SearchDeliveryService(Value, False)
        If Not IsNothing(loDataRow) Then
            p_oDataTable(Row)("sRiderIDx") = loDataRow("sRiderIDx")
            p_oDataTable(Row)("sBriefDsc") = loDataRow("sBriefDsc")

            p_sCompnyNm = loDataRow("sBriefDsc")
            RaiseEvent MasterRetrieved(Row, 1, loDataRow("sBriefDsc"))
        Else
            p_oDataTable(Row)("sRiderIDx") = ""
            p_oDataTable(Row)("sBriefDsc") = ""
            RaiseEvent MasterRetrieved(Row, 1, p_oDataTable(Row)("sBriefDsc"))
        End If

        Return True
    End Function
#End Region

#Region "Private Procedures"
    Private Sub createTable()
        p_oDataTable = New DataTable
        With p_oDataTable
            .Columns.Add("sTransNox", GetType(String)).MaxLength = 20
            .Columns.Add("sRiderIDx", GetType(String)).MaxLength = 3
            .Columns.Add("sRemarksx", GetType(String)).MaxLength = 64
            .Columns.Add("nAmountxx", GetType(Decimal))
            .Columns.Add("sSourceCd", GetType(String)).MaxLength = 4
            .Columns.Add("sSourceNo", GetType(String)).MaxLength = 20
            .Columns.Add("cCollectd", GetType(String)).MaxLength = 1
            .Columns.Add("cBilledxx", GetType(String)).MaxLength = 1
            .Columns.Add("dBilledxx", GetType(Date))
            .Columns.Add("cPaidxxxx", GetType(String)).MaxLength = 1
            .Columns.Add("dPaidxxxx", GetType(Date))
            .Columns.Add("cWaivexxx", GetType(String)).MaxLength = 1
            .Columns.Add("dWaivexxx", GetType(Date))
            .Columns.Add("sWaivexxx", GetType(String))
            .Columns.Add("cTranStat", GetType(String)).MaxLength = 1
            .Columns.Add("sDescript", GetType(String)).MaxLength = 64
            .Columns.Add("sBriefDsc", GetType(String)).MaxLength = 10
        End With
    End Sub

    Private Sub initMaster()
        With p_oDataTable
            .Rows.Add()
            .Rows(0)("sTransNox") = getNextTransNo()
            .Rows(0)("sRiderIDx") = ""
            .Rows(0)("sRemarksx") = ""
            .Rows(0)("nAmountxx") = 0.0
            .Rows(0)("sSourceCd") = p_sSourceCd
            .Rows(0)("sSourceNo") = p_sSourceNo
            .Rows(0)("cTranStat") = Val(xeTranStat.TRANS_OPEN)
            .Rows(0)("sBriefDsc") = ""
        End With
    End Sub

    Private Function isEntryOK(ByVal fnRowNo As Integer) As Boolean
        ' verify the required fields
        If p_oDataTable.Rows(fnRowNo)("sRiderIDx") = String.Empty Then
            MsgBox("Invalid Transaction Company Detected!!!" & vbCrLf & vbCrLf &
                   "Verify your Entry then Try Again!!!", vbCritical, "Warning")
            Return False
        End If

        If p_oDataTable.Rows(fnRowNo)("nAmountxx") <= 0.0 Then
            MsgBox("Invalid Transaction Check Amount Detected!!!" & vbCrLf & vbCrLf &
                   "Verify your Entry then Try Again!!!", vbCritical, "Warning")
            Return False
        End If

        Return True
    End Function
#End Region

#Region "Public Procedures"
    Sub ShowDeliverys()
        p_oFormDelivery = New frmPayDelivery
        With p_oFormDelivery
            .Delivery = Me
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
        p_oDelivery = New clsDeliveryServiceParam(p_oAppDrvr, False)

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

        lsSQL = "SELECT sTransNox" &
                " FROM " & pxeMasterTble &
                " WHERE sTransNox LIKE " & strParm(p_sBranchCd & p_sPOSNo & Format(p_oAppDrvr.getSysDate(), "yy") & "%") &
                " ORDER BY sTransNox DESC" &
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