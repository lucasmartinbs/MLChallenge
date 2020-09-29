Imports System.Text
Imports MLChallenge.Utilidades

Public Class Main
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles BtnSend.Click
        EmailSubject = TBSubject.Text
        EmailTo = TBTo.Text
        EmailBody = RTBody.Text
        Try
            Me.Cursor = Cursors.WaitCursor
            If Utilidades.clsSendMail.SendEMail("BustamanteLucas-MLChallenge", EmailFrom, EmailTo, EmailSubject, EmailBody, EmailFrom, EmailFromPwd, SmtpClient, ArchivoAdjunto) = True Then
                Me.Cursor = Cursors.Default
                MessageBox.Show("El envío de mail ha sido exitoso!", "Envío email", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Clear()
            Else
                Me.Cursor = Cursors.Default
                MessageBox.Show("El envío de mail falló! Verifíque su conexión a internet", "Envío email", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgEx("No se pudo realizar el envío: " + ex.Message)
        End Try

    End Sub
    Private Sub Clear()
        TBTo.Text = "lucasmartinbschallenge@gmail.com"
        TBSubject.Text = Nothing
        RTBody.Text = Nothing
    End Sub

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub


    Private Sub tabcontrol1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        Dim intTabIndex As Integer = TabControl1.SelectedIndex
        Select Case intTabIndex
            Case 0
                Clear()
            Case 1
                GetEmails()
            Case 2
                Me.Cursor = Cursors.WaitCursor
                Me.InboxTableAdapter.Fill(Me.MLChallengeDataSet.inbox)
                Me.Cursor = Cursors.Default
        End Select
    End Sub
    Private Sub GetEmails()
        '************************************
        Dim inboxTableAdapter As New MLChallengeDataSetTableAdapters.inboxTableAdapter()
        '************************************
        inboxTableAdapter.truncate_table()
        '************************************
        Try
            Me.Cursor = Cursors.WaitCursor
            DataGridView1.Rows.Clear()
            '*****************************
            Dim ClientPOP3 As New OpenPop.Pop3.Pop3Client
            ClientPOP3.Connect("pop.gmail.com", "995", True)
            ClientPOP3.Authenticate("lucasmartinbschallenge@gmail.com", "MLChallenge")
            Dim EmailCount As Integer = ClientPOP3.GetMessageCount
            '*********************
            '*************
            For i = EmailCount To 1 Step -1
                Dim message As OpenPop.Mime.Message = ClientPOP3.GetMessage(i)
                Dim Currentrow As Integer = DataGridView1.Rows.Add()
                '*********************************************************
                Try
                    DataGridView1.Rows(Currentrow).Cells("From").Value = message.Headers.From.DisplayName + "(" + message.Headers.From.Address + ")"
                    DataGridView1.Rows(Currentrow).Cells("Subject").Value = message.Headers.Subject
                    DataGridView1.Rows(Currentrow).Cells("Received").Value = message.Headers.DateSent
                    If message.MessagePart.IsMultiPart Then

                        DataGridView1.Rows(Currentrow).Cells("body").Value = message.MessagePart.MessageParts(0).GetBodyAsText
                    Else
                        DataGridView1.Rows(Currentrow).Cells("body").Value = message.MessagePart.GetBodyAsText
                    End If

                Catch ex As Exception
                    MsgEx("No se pudo leer correctamente el mensaje [" + i.ToString + "]")
                End Try

                If SearchInStr(message.Headers.Subject, "DevOps") Or SearchInStr(DataGridView1.Rows(Currentrow).Cells("body").Value, "DevOps") Then
                    Try
                        inboxTableAdapter.insert_message(i, message.Headers.DateSent, DataGridView1.Rows(Currentrow).Cells("From").Value, message.Headers.Subject)
                    Catch ex As Exception
                        MsgEx("No se pudo guardar el mensaje en la base de datos: " + ex.Message)
                    End Try

                End If
            Next
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgEx(ex.Message)
        End Try
        Me.Cursor = Cursors.Default
    End Sub
End Class
