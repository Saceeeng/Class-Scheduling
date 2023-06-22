Imports MySql.Data.MySqlClient
Imports System.Globalization
Public Class Deleteschedule
    Dim cnstr As String = "server=localhost; User=root; password=; database=class_scheduling_system" 'connection string
    Dim sql As String 'variable for storing queries
    Dim conn As New MySqlConnection(cnstr)
    Dim cmd As MySqlCommand
    Dim da As MySqlDataAdapter
    Dim ds As DataSet
    Dim itemcol(99) As String
    Private Sub Deleteschedule_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call quee()
    End Sub
    Public Sub quee()
        Try
            ListView1.Items.Clear()
            conn = New MySqlConnection(cnstr)
            conn.Open()

            Dim sql As String = "SELECT scheduleid,yearlevel,section,roomnumber,instructor,day,TIME_FORMAT(starttime, '%h:%i %p') AS starttime,TIME_FORMAT(endtime, '%h:%i %p') AS endtime from schedule"
            cmd = New MySqlCommand(sql, conn)
            da = New MySqlDataAdapter(cmd)
            ds = New DataSet
            da.Fill(ds, "tables")
            For r = 0 To ds.Tables(0).Rows.Count - 1
                For c = 0 To ds.Tables(0).Columns.Count - 1
                    itemcol(c) = ds.Tables(0).Rows(r)(c).ToString
                Next
                Dim lvitems As New ListViewItem(itemcol)
                ListView1.Items.Add(lvitems)
            Next

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()
    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count > 0 Then
            TextBox1.Text = ListView1.Items(ListView1.SelectedIndices(0)).Text
            TextBox2.Text = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(1).Text
            TextBox3.Text = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(2).Text
            TextBox4.Text = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(3).Text
            TextBox5.Text = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(4).Text
            TextBox6.Text = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(5).Text
            TextBox7.Text = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(6).Text
            TextBox8.Text = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(7).Text

        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim result = MessageBox.Show("Are you sure you want to delete the data?", "Confirmation", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            Try
                conn = New MySqlConnection(cnstr)
                conn.Open()
                Dim sqlQuery = "DELETE FROM schedule WHERE scheduleid =  '" + TextBox1.Text + "'"
                Using command As New MySqlCommand(sqlQuery, conn)
                    command.ExecuteNonQuery()
                End Using
                MsgBox("Data deleted successfully")
                TextBox1.Clear()
                TextBox2.Clear()
                TextBox3.Clear()
                TextBox4.Clear()
                TextBox5.Clear()
                TextBox6.Clear()
                TextBox7.Clear()
                TextBox8.Clear()

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else
            MsgBox("Deletion cancelled")
        End If
        Call quee()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
        scheduler.Show()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
        TextBox7.Clear()
        TextBox8.Clear()
        Call quee()
    End Sub
End Class