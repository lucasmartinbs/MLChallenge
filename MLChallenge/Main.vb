Imports MLChallenge.Utilidades

Public Class Main
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        EmailSubject = TBSubject.Text
        EmailTo = TBTo.Text
        EmailBody = RTBody.Text
        Try
            If Utilidades.clsSendMail.SendEMail("BustamanteLucas-MLChallenge", EmailFrom, EmailTo, EmailSubject, EmailBody, EmailFrom, EmailFromPwd, SmtpClient, ArchivoAdjunto) = True Then
                Me.Cursor = Cursors.Default
                MessageBox.Show("El envío de mail ha sido exitoso!", "Envío email", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Me.Cursor = Cursors.Default
                MessageBox.Show("El envío de mail falló! Verifíque su conexión a internet", "Envío email", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Catch ex As Exception
            MsgEx("No se pudo realizar el envío: " + ex.Message)
        End Try

    End Sub
End Class
