﻿Imports System.Net
Imports System.Net.Mail
Imports System.Text
Imports OpenPop.Pop3
Imports OpenPop.Mime
Imports System.Data

Module Utilidades
    '*************************  PARAMETROS DE SMTP **********************************************
    '*************************  ENVIO EMAIL **********************************************
    Public SmtpClient As String = "smtp.gmail.com"
    Public EmailFrom As String = "lucasmartinbschallenge@gmail.com"
    Public EmailFromPwd As String = "MLChallenge"
    Public EmailPort As Integer = 587
    Public EmailTo As String
    Public EmailSubject As String
    Public EmailBody As String
    Public ArchivoAdjunto As String
    '*************************  RECEPCION EMAIL **********************************************

    '**********************************************************************
    Public Class clsSendMail
        Public Shared Function SendEMail(ByVal strRtte As String, ByVal strOrigen As String, ByVal strDestinatario As String, ByVal strAsunto As String, ByVal strMsg As String, ByVal usuario As String, ByVal Clave As String, ByVal smtp As String, ByVal Adjunto As String) As Boolean
            Dim msg As New MailMessage()
            If strDestinatario.IndexOf(",") > 0 Then
                Dim mail1 As String
                Dim mail2 As String
                Dim len As Integer
                Try
                    mail1 = strDestinatario.Substring(0, strDestinatario.IndexOf(","))
                    len = strDestinatario.Length - strDestinatario.IndexOf(",")
                    mail2 = strDestinatario.Substring(strDestinatario.IndexOf(",") + 1, len - 1)
                    msg.[To].Add(New MailAddress(mail1))
                    msg.[To].Add(New MailAddress(mail2))
                Catch ex As Exception
                    msg.[To].Add(New MailAddress(strDestinatario))
                End Try
            Else
                msg.[To].Add(New MailAddress(strDestinatario))
            End If
            msg.From = New MailAddress("no-reply@sistemascomerciales.net", strRtte)
            'msg.Headers.Add("Reply-to", "no-reply@sistemascomerciales.net")
            msg.ReplyToList.Add("no-reply@sistemascomerciales.net")
            msg.Subject = strAsunto
            msg.Body = strMsg
            'Adjuntar fichero. No se puede ajuntar el fichero tal cual, pues se queda bloqueado.
            If Not Adjunto = Nothing Then
                Dim contentAsBytes As Byte() = Encoding.UTF8.GetBytes(Adjunto)
                Dim memStream As System.IO.MemoryStream = New System.IO.MemoryStream(contentAsBytes)
                '**********************************************************************************
                Dim streamWriter As System.IO.StreamWriter = New System.IO.StreamWriter(memStream)
                streamWriter.Flush()
                '**********************************************************************************
                memStream.Position = 0
                '************************   ADJUNTO 
                Dim fileTXT As String = Adjunto.ToString ' -----------
                Dim data As Net.Mail.Attachment = New Net.Mail.Attachment(fileTXT)
                Dim name As String
                Try
                    name = Adjunto
                    name = name.Substring(InStrRev(name, "\"), name.Length - InStrRev(name, "\"))
                Catch ex As Exception
                    name = "ArchivoAdjunto"
                End Try
                data.Name = name
                '***************************************************************************
                msg.Attachments.Add(data)
                '***************************************************************************
                Dim clienteSmtp As New SmtpClient(smtp)
                clienteSmtp.Credentials = New NetworkCredential(usuario, Clave)
                clienteSmtp.Port = EmailPort
                clienteSmtp.EnableSsl = True
                Try
                    clienteSmtp.Send(msg)
                    data.Dispose()
                    FileSystem.Kill(Adjunto)
                    Return True
                Catch ex As Exception
                    MessageBox.Show("El envío de mail falló: " + ex.Message, "Envío email", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                End Try
            Else
                Dim clienteSmtp As New SmtpClient(smtp)
                clienteSmtp.Credentials = New NetworkCredential(usuario, Clave)
                clienteSmtp.Port = EmailPort
                clienteSmtp.EnableSsl = True
                Try
                    clienteSmtp.Send(msg)
                    Return True
                Catch ex As Exception
                    MessageBox.Show("El envío de mail falló: " + ex.Message, "Envío email", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                End Try
            End If
        End Function
    End Class
    '****************************************************************



    '****************************************************************
    Public Sub MsgEx(ByRef msg As String)
        MessageBox.Show(msg, "Advertencia! [Ex]", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    End Sub
    Public Sub MsgInfo(ByRef msg As String)
        MessageBox.Show(msg, "Aviso! [Info]", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    Public Function SearchInStr(searchString, searchChar) As Boolean
        Dim testPos As Integer
        testPos = InStr(1, searchString, searchChar, CompareMethod.Text)
        If testPos > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Module
