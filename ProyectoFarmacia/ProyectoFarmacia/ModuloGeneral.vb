
Imports System
Imports System.Data
Imports Microsoft.VisualBasic

Imports System.Collections
Imports System.Data.SqlClient
Imports System.IO



Module ModuloGeneral
    Private conexion As New ClassConexion

    'Public Function ObtenerDatosxml() As String

    '    'obtenemos el formato de xml para enviar a transacciones

    '    Dim nombreHost As String = System.Net.Dns.GetHostName
    '    Dim hostInfo As System.Net.IPHostEntry = System.Net.Dns.GetHostByName(nombreHost)
    '    'ObtenerDatos = "<Perfil><Usuario IdEmp='" & idemp & " HostName=' " & hostInfo.HostName.ToString & "' IpHost='" & ip.ToString & "'/></Perfil>"

    '    For Each ip As System.Net.IPAddress In hostInfo.AddressList
    '        ObtenerDatosxml = "<Perfil><Usuario IdEmp='" & idemp & "' HostName=' " & hostInfo.HostName.ToString & "' IpHost='" & ip.ToString & "'/></Perfil>"
    '    Next
    'End Function




    Public Function FechaFormatoSql2(ByVal Dtimerpick As Date) As String
        Dim fecha As String = ""
        Dim dia As String = ""
        Dim anio As String = ""
        Dim mes As String = ""
        anio = Dtimerpick.Year.ToString

        If Len(Dtimerpick.Month.ToString) = 1 Then
            mes = "0" + Dtimerpick.Month.ToString
        Else
            mes = Dtimerpick.Month.ToString
        End If

        If Len(Dtimerpick.Day.ToString) = 1 Then
            dia = "0" + Dtimerpick.Day.ToString
        Else
            dia = Dtimerpick.Day.ToString
        End If
        fecha = dia + "/" + mes + "/" + anio

        Return fecha
    End Function

    Public Function FechaFormatoSql(ByVal Dtimerpick As Date) As String
        Dim fecha As String = ""
        Dim dia As String = ""
        Dim anio As String = ""
        Dim mes As String = ""
        anio = Dtimerpick.Year.ToString

        If Len(Dtimerpick.Month.ToString) = 1 Then
            mes = "0" + Dtimerpick.Month.ToString
        Else
            mes = Dtimerpick.Month.ToString
        End If

        If Len(Dtimerpick.Day.ToString) = 1 Then
            dia = "0" + Dtimerpick.Day.ToString
        Else
            dia = Dtimerpick.Day.ToString
        End If
        fecha = anio + mes + dia

        Return fecha
    End Function


    'Public Sub ReporteMemo(ByVal Memo_id As String)
    '    Dim conexion As New ClassConexion
    '    Dim preview As New VistaPrevia
    '    Dim Report As New RptReporteMemo
    '    Dim dsetrepor As New DtReporteMemo

    '    Dim pnr, pvr As New ArrayList
    '    pnr.Add("@memo_id")
    '    pnr.Add("@opcion")
    '    pvr.Add(Memo_id)
    '    pvr.Add("Encabezado")
    '    dsetrepor = conexion.DataSetReport("RptMemo", dsetrepor, "Encabezado", pvr, pnr, 2, "")

    '    Report.SetDataSource(dsetrepor)



    '    pvr.Item(1) = "Resumen"
    '    dsetrepor = conexion.DataSetReport("RptMemo", dsetrepor, "Resumen", pvr, pnr, 2, "")
    '    Report.SetDataSource(dsetrepor)


    '    pvr.Item(1) = "Detalle"
    '    dsetrepor = conexion.DataSetReport("RptMemo", dsetrepor, "Detalle", pvr, pnr, 2, "")
    '    Report.SetDataSource(dsetrepor)



    '    'Report.SetParameterValue(0, DateEdit1.EditValue)
    '    'Report.SetParameterValue(1, DateEdit2.EditValue)
    '    'MessageBox.Show(dsetrepor.Tables(0).Rows.Count.ToString)
    '    'MessageBox.Show(dsetrepor.Tables(1).Rows.Count.ToString)
    '    'MessageBox.Show(dsetrepor.Tables(2).Rows.Count.ToString)
    '    preview.vistap.ReportSource = Report
    '    preview.Text = "Reporte de Memo"
    '    preview.Show()
    'End Sub

    'Public Sub mensajebox(ByVal titulo As String, ByVal texto As String, ByVal tipo As Integer)
    '    Dim mensaje As New XMensajeOk
    '    mensaje.Label1.Text = titulo
    '    mensaje.Label2.Text = texto

    '    If tipo = 1 Then
    '        mensaje.Pictureok.Visible = True
    '        mensaje.Pictureinfo.Visible = False
    '        mensaje.Picturestop.Visible = False
    '    End If

    '    If tipo = 2 Then
    '        mensaje.Pictureok.Visible = False
    '        mensaje.Label1.ForeColor = Color.Orange

    '        mensaje.Pictureinfo.Visible = True
    '        mensaje.Picturestop.Visible = False

    '    End If

    '    If tipo = 3 Then
    '        mensaje.Label1.ForeColor = Color.Red

    '        mensaje.Pictureok.Visible = False
    '        mensaje.Pictureinfo.Visible = False
    '        mensaje.Picturestop.Visible = True
    '    End If


    '    mensaje.ShowDialog()
    'End Sub

    Public Function bindcombo(ByVal combo As ComboBox, ByVal id As String) As Integer
        Dim i As Integer
        i = -1
        For x = 0 To combo.Items.Count - 1
            combo.SelectedIndex = x
            If combo.SelectedValue.ToString = id Then
                i = x
            End If
        Next

        Return i

    End Function

    Public Function permiso(ByVal formulario As String) As Boolean
        Dim consulta As String
        consulta = "select count(*) as id  from   conf.Menu_Acceso where formulario = '" & formulario & "'"
        Dim dta As New DataTable
        dta = conexion.ConsultaDataTableOnlyQuery(consulta)
        Dim existe As Integer
        existe = CInt(dta.Rows(0).Item(0).ToString)
        If existe = 0 Then
            consulta = "INSERT INTO  conf.Menu_Acceso(formulario,titulo,descripcion,tipo,activo) values('" & formulario & "', '', '', 'PANTALLA',1)"
            conexion.ConsultaDataTableOnlyQuery(consulta)
        End If

        Dim valida As Boolean
        valida = True

        Dim query As String
        query = "select count(*) as id  from conf.Acceso_Perfil co " & _
        " inner join conf.Menu_Acceso  me on co.menu_acceso_id  = me.id  " & _
        "  inner join conf.Perfil_usuario  usu on usu.perfil_id  = co.perfil_id " & _
        " where rtrim(usuario)  = '" & miusuario & "' and RTRIM(formulario) = '" & formulario & "' "
        dta = conexion.ConsultaDataTableOnlyQuery(query)
        existe = CInt(dta.Rows(0).Item(0).ToString)
        If existe = 0 Then
            valida = False
        Else
            valida = True
        End If

        query = "select administrador as permiso from conf.Usuario where usuario ='" & miusuario & "' "
        dta = conexion.ConsultaDataTableOnlyQuery(query)
        Dim permisousuario As Boolean
        permisousuario = CBool(dta.Rows(0).Item(0).ToString)

        If permisousuario = True Then
            valida = True

        End If

        consulta = " select COUNT(*) as id  from conf.Perfil_usuario  pe " & _
        " inner join conf.Usuario  us on pe.usuario = us.usuario  " & _
        " inner join conf.Perfil  per on per.id = pe.perfil_id " & _
        " where per.administador  = 1 and " & _
        " rtrim(us.usuario)  = '" & miusuario & "'"
        dta = conexion.ConsultaDataTableOnlyQuery(consulta)

        existe = CInt(dta.Rows(0).Item(0).ToString)
        If existe > 0 Then
            valida = True
        End If

        If valida = False Then
            MessageBox.Show("rechazado por permisos")
            'mensajebox(formulario, "Opcion Rechazada por falta de permisos   ", 3)
        End If

        Return valida
    End Function


    'Public Function DtRptEncabezado(ByVal titulo As String) As DataSet
    '    Dim dsetrepor As New DtEncabezado

    '    Dim picturemporal As New PictureBox
    '    Dim report As New RptLogotipo
    '    Dim SqlSelect As String = "Select logotipo From cat.empresa Where id = " & sede
    '    Dim Command As New SqlCommand(SqlSelect, conexion.Conexion)
    '    Dim MyPhoto() As Byte = CType(Command.ExecuteScalar(), Byte())
    '    Dim ms As New MemoryStream(MyPhoto)
    '    Try
    '        picturemporal.Image = Image.FromStream(ms)
    '        Dim imagen As New Bitmap(New Bitmap(picturemporal.Image), 145, 145)
    '        picturemporal.Image = imagen
    '    Catch ex As Exception
    '    End Try
    '    Dim temporal As New DataTable
    '    temporal = dsetrepor.Tables(0)
    '    Dim strPathIni As String = My.Application.Info.DirectoryPath
    '    Try
    '        picturemporal.Image.Save(strPathIni & "\templogo.jpg")

    '    Catch ex As Exception
    '        MessageBox.Show(" Permmisos denegado en carpeta  " & strPathIni)
    '    End Try
    '    'MessageBox.Show(strPathIni & "\templogo.jpg")
    '    Try
    '        Dim obtenArchivo As New FileStream(strPathIni & "\templogo.jpg", FileMode.Open)

    '        Dim br As New BinaryReader(obtenArchivo)
    '        Dim imagenBytes As Byte() = New Byte(CInt(obtenArchivo.Length) - 1) {}
    '        br.Read(imagenBytes, 0, CInt(obtenArchivo.Length))
    '        br.Close()
    '        obtenArchivo.Close()
    '        Dim dRow As DataRow = temporal.NewRow()
    '        dRow("reporte") = titulo
    '        dRow("id") = 0
    '        dRow("logotipo") = imagenBytes '6 
    '        temporal.Rows.Add(dRow)

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message.ToString)
    '    End Try
    '    Dim pvr As New ArrayList
    '    Dim pnr As New ArrayList
    '    Dim consulta As String
    '    consulta = " select id,  descripcion,abreviatura,razon_social,direccion,telefono " & _
    '    " ,email,webpage,nit,observaciones,fecha_inicial,no_patronal, fax, " & _
    '    "codigo_actividad_economia, nombre_comercial, gerente_general, auditor_general, jefe_recursos " & _
    '   " from cat.Empresa  where id = " & sede
    '    dsetrepor = conexion.DataSetReport("", dsetrepor, "DetalleEmpresa", pvr, pnr, 1, consulta)

    '    consulta = "select pr.abreviatura,pr.descripcion, ISNULL(dep.descripcion, '') as departamento," & _
    '    " isnull(mun.descripcion, '') as  municipio, direccion,telefono,fax,contacto,  " & _
    '    " CONVERT(varchar(10), fecha_inicial, 103) as fecha_inicial  " & _
    '    "  from cat.Proyecto pr left join cat.Departamento  dep " & _
    '    "  on dep.id = pr.departamento_id  left join cat.Municipio  mun on mun.id = pr.municipio_id  where pr.id =" & proyectoid
    '    dsetrepor = conexion.DataSetReport("", dsetrepor, "Proyecto", pvr, pnr, 1, consulta)

    '    report.SetDataSource(dsetrepor)
    '    Return dsetrepor
    'End Function


    Public Function PermisosConfirm(ByVal clave As String) As Boolean
        Dim valido As Boolean
        Dim data As New DataTable
        valido = False
        data = conexion.ConsultaDataTableOnlyQuery("select COUNT(*)  as  id from conf.usuario where  usuario = '" & miusuario & "' and  clave = '" & clave & "' ")
        Dim existe As Integer
        existe = CInt(data.Rows(0).Item("id").ToString)
        If (existe = 1) Then
            valido = True

        End If

        Return valido

    End Function


End Module
