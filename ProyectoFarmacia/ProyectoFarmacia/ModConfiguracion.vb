Imports Microsoft.VisualBasic
Imports System.Data
Imports System


Module ModConfiguracion
    Public usuariod As String
    Public producto As String
    Public sede As String
    Public sociedad As String
    Public tipo As Integer
    Public rutausuarioxls As String = "C:\catalogos\"
    Public rutausuariodoc As String = "C:\catalogos\"
    Public descargarh As String = "C:\catalogos\"
    Public nombreempresa As String
    Public miusuario As String '= "Administrador"
    Public proyectoid As String
    Public nombreproyecto As String
    Public resultado As Integer = 0
    Public seleccionplanilla As Integer = 0
    Public PLanillaInterna As Integer = 0
    Public fechapla As String



    'Este módulo se utiliza para cargar las configuraciones generales que el sistema utilizará
    'mientras la sesión esté abierta
    Public strConexion As String 'String de conexion que se utiliza en todo el proyecto
    ' Leer una clave de un fichero INI
    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" _
    (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, _
    ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
    Public Function IniGet(ByVal sFileName As String, ByVal sSection As String, ByVal sKeyName As String, Optional ByVal sDefault As String = "") As String
        '--------------------------------------------------------------------------
        ' Devuelve el valor de una clave de un fichero INI
        ' Los parámetros son:
        '   sFileName   El fichero INI
        '   sSection    La sección de la que se quiere leer
        '   sKeyName    Clave
        '   sDefault    Valor opcional que devolverá si no se encuentra la clave
        '--------------------------------------------------------------------------        
        Dim ret As Integer
        'Dim sRetVal As String        '
        Dim sRetVal As String = New String(Chr(0), 255)
        'sRetVal = Space(100)
        ret = GetPrivateProfileString(sSection, sKeyName, sDefault, sRetVal, Len(sRetVal), sFileName)
        If ret = 0 Then
            Return sDefault
        Else
            Return Microsoft.VisualBasic.Left(sRetVal, ret)
        End If
    End Function


End Module
