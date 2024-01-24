'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     RetMgtSys Payment
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

Public Class Payment

#Region "Payment Form"
    Private Enum xePaymForm
        xeCash = 0
        xeCreditCard = 1
        xeCheck = 2
        xeGiftCertificate = 3
    End Enum
#End Region

#Region "Constant"
    Private Const pxeMODULENAME As String = "Payment"
    Private Const pxeMasterTble As String = "Payment"
#End Region

#Region "Protected Members"
    Protected p_oAppDrvr As GRider
    Protected p_oDataTable As DataTable
    Protected p_nEditMode As xeEditMode

    Protected p_sBranchCd As String
    Protected p_sSourceNo As String
    Protected p_sSourceCd As String
#End Region

#Region "Properties"
    Property Branch() As String
        Get
            Return p_sBranchCd
        End Get
        Set(ByVal Value As String)
            p_sBranchCd = Value
        End Set
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

    Property Master(ByVal Row As Integer, _
                    ByVal Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then Index = LCase(Index)
            Select Case Index
                Case "stransnox" : Index = 0
                Case "nentrynox" : Index = 1
                Case "cpaymform" : Index = 2
                Case "namountxx" : Index = 3
                Case Else
                    MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                    Return DBNull.Value
            End Select
            Return p_oDataTable(Row)(Index)
        End Get

        Set(ByVal Value As Object)
            Select Case Index
                Case "stransnox" : Index = 0
                Case "nentrynox" : Index = 1
                Case "cpaymform" : Index = 2
                Case "namountxx" : Index = 3
                Case Else
                    MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
            End Select
            p_oDataTable(Row)(Index) = Value
        End Set
    End Property

    ReadOnly Property ItemCount() As Long
        Get
            Return p_oDataTable.Rows.Count
        End Get
    End Property
#End Region

#Region "Public Function"
    Function NewTransaction() As Boolean
        Call createTable()

        Return True
    End Function

    Function SaveTransaction() As Boolean
        Dim lsSQL As String
        Dim lnRow As Integer
        Dim lnCtr As Integer

        With p_oDataTable
            For lnCtr = 0 To .Rows.Count - 1
                lsSQL = "INSERT INTO " & pxeMasterTble & _
                            " SET sTransNox = " & strParm(p_oDataTable.Rows(lnCtr)("sTransNox")) & _
                            ", nEntryNox = " & CInt(lnCtr + 1) & _
                            ", cPaymForm = " & strParm(p_oDataTable.Rows(lnCtr)("cPaymForm")) & _
                            ", nAmountxx = " & CDec(p_oDataTable.Rows(lnCtr)("nAmountxx")) & _
                            ", sSourceCd = " & strParm(p_sSourceCd) & _
                            ", sSourceno = " & strParm(p_sSourceNo)

                Try
                    Debug.Print(lsSQL)

                    lnRow = p_oAppDrvr.Execute(lsSQL, pxeMasterTble)
                    If lnRow <= 0 Then
                        MsgBox("Unable to Save Transaction!!!" & vbCrLf & _
                                "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                        Return False
                    End If
                Catch ex As Exception
                    MsgBox(ex.Message)
                    Throw ex
                End Try
            Next lnCtr
        End With

        Return True
    End Function

    Function AddPayment() As Boolean
        Dim lnRow As Integer = p_oDataTable.Rows.Count

        With p_oDataTable
            .Rows.Add()
            .Rows(lnRow)("sTransNox") = ""
            .Rows(lnRow)("nEntryNox") = .Rows.Count
            .Rows(lnRow)("cPaymForm") = 1
            .Rows(lnRow)("nAmountxx") = 0.0
        End With

        Return True
    End Function
#End Region

#Region "Private function"
    'Private Function
    Private Function getSQ_Master() As String
        Return "SELECT" & _
                    "  sTransNox" & _
                    ", nEntryNox" & _
                    ", cPaymForm" & _
                    ", nAmountxx" & _
              " FROM " & pxeMasterTble
    End Function
#End Region

#Region "Private Procedures"
    Private Sub createTable()
        p_oDataTable = New DataTable
        With p_oDataTable
            .Columns.Add("sTransNox", GetType(System.String)).MaxLength = 20
            .Columns.Add("nEntryNox", GetType(System.Int32))
            .Columns.Add("cPaymForm", GetType(System.String)).MaxLength = 1
            .Columns.Add("nAmountxx", GetType(System.Decimal))
        End With
    End Sub
#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub New(ByVal foRider As GRider)
        p_oAppDrvr = foRider
        If p_sBranchCd = String.Empty Then p_sBranchCd = p_oAppDrvr.BranchCode
        p_nEditMode = xeEditMode.MODE_UNKNOWN
    End Sub
End Class