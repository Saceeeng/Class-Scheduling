Imports MySql.Data.MySqlClient
Public Class Add_Instructor
    Dim cnstr As String = "server=localhost; User=root; password=; database=class_scheduling_system"
    Dim conn As New MySqlConnection(cnstr)
    Dim cmd As MySqlCommand
    Dim da As MySqlDataAdapter
    Dim ds As DataSet
    Dim itemcol(99) As String
    Private Sub Add_Instructor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call quee()
    End Sub
    Public Sub quee()
        Try
            ListView1.Items.Clear()
            conn = New MySqlConnection(cnstr)
            conn.Open()
            Dim sql As String = "SELECT * FROM instructor"
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
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
            Try
                conn = New MySqlConnection(cnstr)
                conn.Open()
                Dim sql As String = "INSERT INTO instructor (instructorname,instructoryr) VALUES (@instructorname,@instructoryr)"
                cmd = New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@instructorname", TextBox1.Text)
                cmd.Parameters.AddWithValue("@instructoryr", ComboBox1.SelectedItem)
                Dim i As Integer = cmd.ExecuteNonQuery
                If i > 0 Then
                    MsgBox("INSTRUCTOR REGISTERED", vbInformation, "Query")
                Else
                    MsgBox("INSTRUCTOR NOT REGISTERED:", vbInformation, "Query")
                End If
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
        End Try
      
        ComboBox1.SelectedIndex = -1
        TextBox1.Clear()
        Call quee()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        ComboBox2.SelectedIndex = -1
        TextBox2.Clear()
        Call quee()
    End Sub
    Dim tempinstructorid As String = 0
    Dim tempname As String
    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
    If ListView1.SelectedItems.Count > 0 Then
            tempinstructorid = ListView1.Items(ListView1.SelectedIndices(0)).Text
            TextBox2.Text = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(1).Text
            tempname = TextBox2.Text
            ComboBox2.SelectedItem = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(2).Text
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim result = MessageBox.Show("Are you sure you want to update the data?", "Confirmation", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            conn = New MySqlConnection(cnstr)
            conn.Open()
            Dim query As String = "UPDATE instructor SET instructorname = @instructorname WHERE instructorid = '" + tempinstructorid + "' " ' Adjust column names and query as per your table structure
            Using command As New MySqlCommand(query, conn)
                command.Parameters.AddWithValue("@instructorname", TextBox2.Text)
                command.ExecuteNonQuery()
            End Using
            conn.Close()
            conn = New MySqlConnection(cnstr)
            conn.Open()
            query = "UPDATE schedule SET instructor = @instructor WHERE instructor = '" + tempname.ToString + "' " ' Adjust column names and query as per your table structure
            Using command As New MySqlCommand(query, conn)
                command.Parameters.AddWithValue("@instructor", TextBox2.Text)
                MsgBox("Succesful Update")
                command.ExecuteNonQuery()
            End Using
            conn.Close()
        Else

        End If
        ComboBox2.SelectedIndex = -1
        TextBox2.Clear()
        Call quee()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim result = MessageBox.Show("Are you sure you want to delete the data? all data of instructor will be deleted including schedules.", "Confirmation", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            Try
                conn = New MySqlConnection(cnstr)
                conn.Open()
                Dim sqlQuery = "DELETE FROM schedule WHERE instructor =  '" + tempname + "'"
                Using command As New MySqlCommand(sqlQuery, conn)
                    command.ExecuteNonQuery()
                End Using
                TextBox2.Clear()     
                Call quee()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            Try
                conn = New MySqlConnection(cnstr)
                conn.Open()
                Dim sqlQuery = "DELETE FROM instructor WHERE instructorname =  '" + tempname + "'"
                Using command As New MySqlCommand(sqlQuery, conn)
                    command.ExecuteNonQuery()
                End Using
                MsgBox("Data deleted successfully")
                TextBox2.Clear()
                Call quee()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else
            MsgBox("Deletion cancelled")
        End If
        ComboBox2.SelectedIndex = -1
        TextBox2.Clear()
        Call quee()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Me.Close()
        Form1.Show()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        ComboBox1.SelectedIndex = -1
        TextBox1.Clear()
        Call quee()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Try
            ListView1.Items.Clear()
            conn = New MySqlConnection(cnstr)
            conn.Open()
            Dim sql As String = "SELECT * FROM instructor WHERE instructoryr = '" + ComboBox1.SelectedItem + "' "
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
End Class