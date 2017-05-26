Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections
Imports System.Text

Imports Microsoft.VisualBasic
Public Class ClassConexion

    Public Function Conexion() As System.Data.SqlClient.SqlConnection
        'Esta funcion conecta y abre la conexion a la Base de Datos
        Dim StrConexionString As String = strConexion
        Dim cnSql As SqlConnection = New System.Data.SqlClient.SqlConnection(StrConexionString)
        cnSql.Open()
        Return cnSQL
    End Function
    Public Sub ParametrosConexion(ByVal User As String, ByVal PW As String, ByVal bd As String)
        'Este procemiento arma la cadena de conexion luego de haber sido leidos del .INI
        Dim strPathIni As String = My.Application.Info.DirectoryPath  'Path donde se va a poner el .Ini, falta definirlo
        Dim strFileIni As String = "\conexio.xml" 'Path del archivo .ini

        Dim cadenas As String = strPathIni + strFileIni

        Dim data As New DataSet
        data.ReadXml(cadenas)

        '    

        'Public sserver As String = "(local)"
        'Public Sbd As String = "control"

        Dim sserver As String = data.Tables("CONEXION").Rows(0).Item("servidor").ToString()
        'Dim Sbd As String = data.Tables(0).Rows(0).Item("base").ToString()
        Dim usuario As String = data.Tables("CONEXION").Rows(0).Item("usuario").ToString()
        Dim contra As String = data.Tables("CONEXION").Rows(0).Item("contra").ToString()




        Dim cadenaConexion As New StringBuilder
        With cadenaConexion
            .Append("User Id=")
            .Append(usuario)
            .Append(";Pwd =")
            .Append(contra)
            .Append(";Data source=")
            .Append(sserver + ";")
            .Append("Persist Security Info=False;Initial Catalog=")
            .Append(bd)
            '.Append("Persist Security Info=False;Integrated Security=SSPI;Data source=Doctor;server=ABRAHAM-PC\SQLEXPRESS")


        End With
        strConexion = ""
        strConexion = cadenaConexion.ToString
        'MessageBox.Show(strConexion)
    End Sub
    Public Function ExecCommand(ByVal NombreSp As String, ByVal ParametrosSpValores As ArrayList, ByVal ParametrosSpNombres As ArrayList, ByVal TipodeDato As Integer, ByVal Query As String) As SqlCommand

        ' armar command para execucion segun tipo de datos 1 query 2 sp
        Dim Cmd As New SqlCommand
        Dim Cont As Integer
        Try
            Cmd.Connection = Conexion()
            Cmd.CommandTimeout = 300
            Select Case TipodeDato ' Valor recibido por parametro
                Case 1 ' Ejecuta Query
                    Cmd.CommandType = Data.CommandType.Text
                    Cmd.CommandText = Query
                Case 2 ' Ejecuta SP
                    Cmd.CommandType = Data.CommandType.StoredProcedure
                    Cmd.CommandText = NombreSp
                    ' asignamos valores al sp
                    Cmd.Parameters.Clear()
                    For Cont = 0 To ParametrosSpValores.Count - 1
                        Cmd.Parameters.AddWithValue(ParametrosSpNombres.Item(Cont).ToString, ParametrosSpValores.Item(Cont))
                    Next

            End Select
        Catch ex As Exception
            MsgBox("Error de conexion", MsgBoxStyle.Information, "Procedimiento:" & NombreSp)

        End Try

        Conexion.Close()


        Return Cmd
        Cmd.Connection.Close()

    End Function
    Public Function ConsultaDataTablesp(ByVal Procedimiento As String, ByVal ParametrosValores As ArrayList, ByVal ParametrosNombres As ArrayList) As DataTable

        ' Retorna consulta en datatable
        Dim Dta As New SqlDataAdapter
        Dim Dt As New DataTable
        Try
            Dta.SelectCommand = ExecCommand(Procedimiento, ParametrosValores, ParametrosNombres, 2, "")
            Dta.Fill(Dt)

        Catch ex As SqlException
            MsgBox(ex.ToString, MsgBoxStyle.Information)

        Finally
            If (Dta.SelectCommand.Connection.State = ConnectionState.Open) Or (Dta.SelectCommand.Connection.State = ConnectionState.Broken) Then
                Dta.SelectCommand.Connection.Close()
            End If
        End Try
        Dta.Dispose()
        Return Dt
    End Function
   
    Public Function ConsultaReader(ByVal SPName As String, ByVal SpParameterValue As ArrayList, ByVal SpParameterName As ArrayList, ByVal CommandType As Integer, ByVal Query As String, ByVal forma As String, ByVal nolinea As Integer) As Data.DataTableReader

        Dim Reader As DataTableReader
        Dim Dta As New SqlDataAdapter
        Dim Dt As New DataTable

        Try

            Dta.Fill(Dt)
            Reader = New DataTableReader(Dt)

        Catch ex As Exception
            'MsgBox(ex.Message)
            Reader = New DataTableReader(Dt)

        Finally
            If (Dta.SelectCommand.Connection.State = ConnectionState.Open) Or (Dta.SelectCommand.Connection.State = ConnectionState.Broken) Then
                Dta.SelectCommand.Connection.Close()
            End If
        End Try
        Dta.Dispose()
        Return Reader
    End Function

    Public Function NoQueryTransac(ByVal SPNombre As String, ByVal ParametrosValor As ArrayList, ByVal ParametrosNombre As ArrayList, ByVal Tipo As Integer, ByVal Query As String) As Boolean

        Dim Cmd As New SqlCommand
        Dim Trans As SqlTransaction

        Cmd = ExecCommand(SPNombre, ParametrosValor, ParametrosNombre, Tipo, Query)
        Trans = Cmd.Connection.BeginTransaction

        Try
            Cmd.Transaction = Trans
            Cmd.ExecuteNonQuery()
            Trans.Commit()
            Return True
        Catch ex As Exception
            Trans.Rollback()
            'MsgBox(ex.Message)

            ' Trans.Commit()
            ' MsgBox(Err.Number & " " & Err.Description, "Error_:" + ex.ToString)
            Return False
        Finally
            If (Cmd.Connection.State = ConnectionState.Open) Or (Cmd.Connection.State = ConnectionState.Broken) Then
                Cmd.Connection.Close()
                Cmd = Nothing
                Trans = Nothing
            End If
        End Try
    End Function
    Public Function DataSetReport(ByVal SPNombre As String, ByRef dtSet As DataSet, ByVal DtTable As String, ByVal ParametrosValor As ArrayList, ByVal ParametrosNombre As ArrayList, ByVal CommandType As Integer, ByVal Query As String) As DataSet
        Dim DTA As New SqlDataAdapter
        Try
            DTA.SelectCommand = ExecCommand(SPNombre, ParametrosValor, ParametrosNombre, CommandType, Query)
            DTA.Fill(dtSet, DtTable)

        Catch ex As Exception
            'MsgBox(ex.Message)



        Finally
            If (DTA.SelectCommand.Connection.State = ConnectionState.Open) Or (DTA.SelectCommand.Connection.State = ConnectionState.Broken) Then
                DTA.SelectCommand.Connection.Close()
            End If
        End Try
        DTA.Dispose()
        Return dtSet
    End Function
   
    Public Function ConsultaDataSet(ByVal SP As String, ByVal PValores As ArrayList, ByVal Pnombres As ArrayList, ByVal Comando As Integer, ByVal Query As String, ByVal NombreDset As String) As Data.DataSet
        Dim DTA As New SqlDataAdapter
        Dim DS As New DataSet
        Try
            DTA.SelectCommand = ExecCommand(SP, PValores, Pnombres, Comando, Query)
            DTA.Fill(DS, NombreDset.ToString)
        Catch ex As Exception
            'MsgBox(ex.Message)
        Finally
            If (DTA.SelectCommand.Connection.State = ConnectionState.Open) Or (DTA.SelectCommand.Connection.State = ConnectionState.Broken) Then
                DTA.SelectCommand.Connection.Close()
            End If
        End Try
        Return DS
        DTA.Dispose()
    End Function

    Public Function ConsultaDataSetOnlyQuery(ByVal Query As String, ByVal NombreDset As String) As Data.DataSet
        Dim DTA As New SqlDataAdapter
        Dim P As New ArrayList
        Dim DS As New DataSet
        Try
            DTA.SelectCommand = ExecCommand("", P, P, 1, Query)
            DTA.Fill(DS, NombreDset.ToString)
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            If (DTA.SelectCommand.Connection.State = ConnectionState.Open) Or (DTA.SelectCommand.Connection.State = ConnectionState.Broken) Then
                DTA.SelectCommand.Connection.Close()
            End If
        End Try
        Return DS
        DTA.Dispose()
    End Function

    Public Function ConsultaDataTableOnlyQuery(ByVal Query As String) As DataTable
        ' Retorna consulta en datatable
        Dim Dta As New SqlDataAdapter
        Dim p As New ArrayList
        Dim Dt As New DataTable
        Try
            Dta.SelectCommand = ExecCommand("", p, p, 1, Query)
            Dta.Fill(Dt)

        Catch ex As Exception
            MsgBox(" " & ex.Message & " " & Query, MsgBoxStyle.Information, "Consulta:" & Query & " Dta")

        Finally
            If (Dta.SelectCommand.Connection.State = ConnectionState.Open) Or (Dta.SelectCommand.Connection.State = ConnectionState.Broken) Then
                Dta.SelectCommand.Connection.Close()
            End If
        End Try
        Dta.Dispose()
        Return Dt
    End Function


    Public Function EliminaRegistro(ByVal tabla As String, ByVal id As String) As Boolean
        ' Retorna consulta en datatable
        Dim Dta As New SqlDataAdapter
        Dim p As New ArrayList
        Dim Dt As New DataTable
        Dim query As String
        Dim eliminado As Boolean

        query = "delete from " & tabla & " where id = " & id

        Try
            Dta.SelectCommand = ExecCommand("", p, p, 1, query)
            Dta.Fill(Dt)
            eliminado = True

        Catch ex As Exception
              MsgBox("Debe de desvincular las relaciones que tiene este registro ", MsgBoxStyle.Exclamation, "Imposible eliminar")
            eliminado = False
        Finally
            If (Dta.SelectCommand.Connection.State = ConnectionState.Open) Or (Dta.SelectCommand.Connection.State = ConnectionState.Broken) Then
                Dta.SelectCommand.Connection.Close()
            End If
        End Try
        Dta.Dispose()
        Return eliminado

    End Function



End Class
