Public Class Form1
    Private conexion As New ClassConexion

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load



        '-----------***** CONEXION INICIAL UNA SOLA VEZ
        Try
            Dim data As New DataSet
            'proyectoid = ""
            Dim strPathIni As String = My.Application.Info.DirectoryPath  'Path donde se va a poner el .Ini, falta definirlo
            'MessageBox.Show(strPathIni)

            Dim strFileIni As String = "\conexio.xml" 'Path del archivo .ini
            Dim cadenas As String = strPathIni + strFileIni

            data.ReadXml(cadenas)
            Dim usuarioservidor = data.Tables("CONEXION").Rows(0).Item("USUARIO").ToString()
            Dim basedatos As String = data.Tables("CONEXION").Rows(0).Item("BASE").ToString()
            Dim contrasevidor = data.Tables("CONEXION").Rows(0).Item("CONTRA").ToString()
            Dim sevidor = data.Tables("CONEXION").Rows(0).Item("SERVIDOR").ToString()
         
            conexion.ParametrosConexion(usuarioservidor, contrasevidor, basedatos)
            conexion.Conexion()
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString, "Problemas con la conexion")
            Application.Exit()
        End Try
        ''''  ------------****


       

        carga()
        habilita(False)



    End Sub


    Public Sub carga()
        Dim consulta As String
        consulta = "select  nombre, nit, direccion, telefono,id from cliente"
        Dim dataconsulta As New DataTable
        dataconsulta = conexion.ConsultaDataTableOnlyQuery(consulta)
        DataGridView1.DataSource = dataconsulta
        DataGridView1.Refresh()

    End Sub


    Private Sub BtBusca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtBusca.Click

        Dim consulta As String
        consulta = "select  nombre, nit, direccion, telefono,id from cliente where nombre like '%" & Trim(txtbusqueda.Text) & "%'"
        Dim dataconsulta As New DataTable
        dataconsulta = conexion.ConsultaDataTableOnlyQuery(consulta)
        DataGridView1.DataSource = dataconsulta
        DataGridView1.Refresh()




    End Sub

    Private Sub BtBuscaSp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtBuscaSp.Click

        Dim pnr, pvr As New ArrayList
        pnr.Add("@busca")
        pvr.Add(txtbusqueda.Text)

        Dim dataconsulta As New DataTable

        dataconsulta = conexion.ConsultaDataTablesp("buscaproducto", pvr, pnr)
        DataGridView1.DataSource = dataconsulta
        DataGridView1.Refresh()



    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtnombre.TextChanged

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim pnr, pvr As New ArrayList

        pnr.Add("@nombre") ' nvarchar(100),
        pvr.Add(txtnombre.Text)
        pnr.Add("@nit") ' nchar(10),
        pvr.Add(txtnit.Text)
        pnr.Add("@direccion") ' nvarchar(100),
        pvr.Add(txtdireccion.Text)
        pnr.Add("@telefono") ' nchar(10)
        pvr.Add(txttelefono.Text)

        conexion.ConsultaDataTablesp("insetcliente", pvr, pnr)


        MessageBox.Show("Informacion almacenada con exito")
        carga()
        limipeza()
    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim consulta As String

        consulta = "UPDATE cliente set " & _
       " nombre = '" & txtnombre.Text & "'" & _
      " ,nit =' " & txtnit.Text & "' " & _
       ",direccion = '" & txtdireccion.Text & "', " & _
       "  telefono = '" & txttelefono.Text & "' " & _
        " WHERE id =  " & txtId.Text

        conexion.ConsultaDataTableOnlyQuery(consulta)
        MessageBox.Show("Informacion almacenada con exito")
        carga()

        limipeza()

    End Sub



    Public Sub habilita(ByVal valido As Boolean)
        txtdireccion.Enabled = valido '
        txtnit.Enabled = valido '.Text = ""

        txtnombre.Enabled = valido '.Text = ""
        txttelefono.Enabled = valido '.Text = ""
        txtId.Enabled = valido '
    End Sub


    Public Sub limipeza()
        txtdireccion.Text = ""
        txtnit.Text = ""

        txtnombre.Text = ""
        txttelefono.Text = ""
    End Sub


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        Dim valido As Boolean = True
        Dim entero As Integer
        If txtId.Text = "" Then
            ErrorProvider1.SetError(txtId, "debes ingresar valor")
            valido = False
        End If
        Try
            entero = CInt(txtId.Text)
        Catch ex As Exception
            ErrorProvider1.SetError(txtId, "debes ingresar valor entero")
            valido = False

        End Try

        If valido = True Then
            If MessageBox.Show("Esta seguro eliminar registro?", "Eliminar", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then

                Dim consultas As String
                consultas = "delete from cliente where id = " & txtId.Text
                conexion.ConsultaDataTableOnlyQuery(consultas)

                carga()

            End If

        End If


    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        habilita(CheckBox1.Checked)
    End Sub
End Class
