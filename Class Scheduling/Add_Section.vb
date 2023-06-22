Imports MySql.Data.MySqlClient
Public Class Add_Section
    Dim cnstr As String = "server=localhost; User=root; password=; database=class_scheduling_system"
    Dim conn As New MySqlConnection(cnstr)
    Dim cmd As MySqlCommand
    Dim da As MySqlDataAdapter
    Dim ds As DataSet
    Dim itemcol(99) As String
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        ComboBox3.SelectedIndex = -1
        TextBox3.Clear()
        Call quee()
    End Sub
    Public Sub quee()
        Try
            ListView1.Items.Clear()
            conn = New MySqlConnection(cnstr)
            conn.Open()
            Dim sql As String = "SELECT * FROM sections"
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
        Try
            ListView2.Items.Clear()
            conn = New MySqlConnection(cnstr)
            conn.Open()
            Dim sql As String = "SELECT * FROM room"
            cmd = New MySqlCommand(sql, conn)
            da = New MySqlDataAdapter(cmd)
            ds = New DataSet
            da.Fill(ds, "tables")
            For r = 0 To ds.Tables(0).Rows.Count - 1
                For c = 0 To ds.Tables(0).Columns.Count - 1
                    itemcol(c) = ds.Tables(0).Rows(r)(c).ToString
                Next
                Dim lvitems As New ListViewItem(itemcol)
                ListView2.Items.Add(lvitems)
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
            Dim sql As String = "INSERT INTO sections (yearlevel,section) VALUES (@year,@section)"
            cmd = New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@year", ComboBox1.SelectedItem)
            cmd.Parameters.AddWithValue("@section", TextBox1.Text)


            Dim i As Integer = cmd.ExecuteNonQuery
            If i > 0 Then
                MsgBox("SECTION ADDED", vbInformation, "Query")
            Else
                MsgBox("SECTION NOT ADDED:", vbInformation, "Query")
            End If
            conn.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        ComboBox1.SelectedIndex = -1
        TextBox1.Clear()
        Call quee()
    End Sub

    Private Sub Add_Section_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call quee()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Try
            conn = New MySqlConnection(cnstr)
            conn.Open()
            Dim sql As String = "INSERT INTO room (yearlevel,roomnumber) VALUES (@year,@roomnumber)"
            cmd = New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@year", ComboBox2.SelectedItem)
            cmd.Parameters.AddWithValue("@roomnumber", TextBox2.Text)


            Dim i As Integer = cmd.ExecuteNonQuery
            If i > 0 Then
                MsgBox("ROOM ADDED", vbInformation, "Query")
            Else
                MsgBox("ROOM NOT ADDED:", vbInformation, "Query")
            End If
            conn.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        ComboBox2.SelectedIndex = -1
        TextBox2.Clear()
        Call quee()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Dim result = MessageBox.Show("Are you sure you want to delete the data? all data of section will be deleted including schedules.", "Confirmation", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            Try
                conn = New MySqlConnection(cnstr)
                conn.Open()
                Dim sqlQuery = "DELETE FROM schedule WHERE section =  '" + tempsection + "'"
                Using command As New MySqlCommand(sqlQuery, conn)
                    command.ExecuteNonQuery()
                End Using

                Call quee()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            Try
                conn = New MySqlConnection(cnstr)
                conn.Open()
                Dim sqlQuery = "DELETE FROM sections WHERE sectionid =  '" + tempsectionid + "'"
                Using command As New MySqlCommand(sqlQuery, conn)
                    command.ExecuteNonQuery()
                End Using
                MsgBox("Data deleted successfully")
                TextBox3.Clear()
                Call quee()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else
            MsgBox("Deletion cancelled")
        End If
        ComboBox3.SelectedIndex = -1
        TextBox3.Clear()
        Call quee()
    End Sub
    Dim tempsectionid As String
    Dim tempsection As String
    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count > 0 Then
            tempsectionid = ListView1.Items(ListView1.SelectedIndices(0)).Text
            ComboBox3.SelectedItem = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(1).Text
            TextBox3.Text = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(2).Text
            tempsection = TextBox3.Text
        End If

    End Sub

    Private Sub Button5_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Me.Close()
        Form1.Show()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        ComboBox2.SelectedIndex = -1
        TextBox2.Clear()
        Call quee()
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        ComboBox1.SelectedIndex = -1
        TextBox1.Clear()
        Call quee()
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        If TabControl1.SelectedTab Is TabPage1 Then
            ComboBox2.SelectedIndex = -1
            TextBox2.Clear()
            Call quee()
        Else
            ComboBox1.SelectedIndex = -1
            TextBox1.Clear()
            ComboBox3.SelectedIndex = -1
            TextBox3.Clear()
            Call quee()
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Try
            ListView1.Items.Clear()
            conn = New MySqlConnection(cnstr)
            conn.Open()
            Dim sql As String = "SELECT * FROM sections WHERE yearlevel =  '" + ComboBox1.SelectedItem + "'"
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

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        Try
            ListView2.Items.Clear()
            conn = New MySqlConnection(cnstr)
            conn.Open()
            Dim sql As String = "SELECT * FROM room WHERE yearlevel = '" + ComboBox2.SelectedItem + "'"
            cmd = New MySqlCommand(sql, conn)
            da = New MySqlDataAdapter(cmd)
            ds = New DataSet
            da.Fill(ds, "tables")
            For r = 0 To ds.Tables(0).Rows.Count - 1
                For c = 0 To ds.Tables(0).Columns.Count - 1
                    itemcol(c) = ds.Tables(0).Rows(r)(c).ToString
                Next
                Dim lvitems As New ListViewItem(itemcol)
                ListView2.Items.Add(lvitems)
            Next

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()
    End Sub
End Class