Imports MySql.Data.MySqlClient
Imports System.Globalization
Public Class scheduler
    Dim cnstr As String = "server=localhost; User=root; password=; database=class_scheduling_system" 'connection string
    Dim sql As String 'variable for storing queries
    Dim conn As New MySqlConnection(cnstr)
    Dim cmd As MySqlCommand
    Dim da As MySqlDataAdapter
    Dim ds As DataSet
    Dim itemcol(99) As String
    Private Sub firstyear_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call get_all_data() ' call function for collecting data from instructor, room and section
    End Sub

    'function for collecting data
    Public Sub get_all_data()
        Try
            conn.Open()
            Dim query As String = "SELECT * FROM instructor WHERE instructoryr = '" + Label8.Text + "' " 'for instructor
            Using command As New MySqlCommand(query, conn)
                Using reader As MySqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim instructorName As String = reader.GetString("instructorname")
                        ComboBox2.Items.Add(instructorName)
                        ComboBox6.Items.Add(instructorName)
                    End While
                End Using
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()
        Try
            conn.Open()
            Dim query As String = "SELECT * FROM sections WHERE yearlevel = '" + Label8.Text + "' " 'for section
            Using command As New MySqlCommand(query, conn)
                Using reader As MySqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim sectionname As String = reader.GetString("section")
                        ComboBox1.Items.Add(sectionname)
                        ComboBox5.Items.Add(sectionname)
                    End While
                End Using
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()
        Try
            conn.Open()
            Dim query As String = "SELECT * FROM room WHERE yearlevel = '" + Label8.Text + "' " 'for room
            Using command As New MySqlCommand(query, conn)
                Using reader As MySqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim roomnumber As String = reader.GetString("roomnumber")
                        ComboBox8.Items.Add(roomnumber)
                        ComboBox7.Items.Add(roomnumber)
                    End While
                End Using
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()
        Call quee()
    End Sub

    'function for refreshing listview
    Public Sub quee()
        Try
            ListView1.Items.Clear()
            conn = New MySqlConnection(cnstr)
            conn.Open()
            Dim sql As String = "SELECT scheduleid,yearlevel,section,roomnumber,instructor,day,TIME_FORMAT(starttime, '%h:%i %p') AS starttime,TIME_FORMAT(endtime, '%h:%i %p') AS endtime from schedule where yearlevel = '" + Label8.Text + "' "

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

    Dim stime As String 'variable for start time
    Dim etime As String 'variable for end time

    'Submiting schedule
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim bool As Boolean
        Dim connectionString As String = "server=localhost; User=root; password=; database=class_scheduling_system" ' connection string
        Dim tableName As String = "Schedules" ' table name
        Dim timeString As String = TextBox1.Text.Replace(" ", "")
        Dim format As String = "h:mmtt"  ' Format for 12-hour clock with AM/PM indicator
        Dim parsedTime As DateTime
        If DateTime.TryParseExact(timeString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, parsedTime) Then
            Dim dateTimeValue As DateTime = parsedTime
            Dim timeValue As TimeSpan = dateTimeValue.TimeOfDay
            stime = timeValue.ToString
            'MsgBox(timeValue.ToString)
        Else
            MsgBox("Invalid time format")
            GoTo ending
        End If
        If DateTime.TryParseExact(TextBox2.Text.Replace(" ", ""), format, CultureInfo.InvariantCulture, DateTimeStyles.None, parsedTime) Then
            Dim dateTimeValue As DateTime = parsedTime
            Dim timeValue As TimeSpan = dateTimeValue.TimeOfDay
            etime = timeValue.ToString
            'MsgBox(timeValue.ToString)
        Else
            MsgBox("Invalid time format")
            GoTo ending
        End If

        'finding if there is conflict in schedules
        Try
            Dim sqlQuery As String = "SELECT * FROM schedule WHERE roomnumber = '" + ComboBox7.SelectedItem + "' AND day =  '" + ComboBox3.SelectedItem + "' AND  '" + TextBox1.Text + "'BETWEEn starttime and endtime" ' Replace with your MySQL query
            Using connection As New MySqlConnection(connectionString)
                connection.Open()
                Using command As New MySqlCommand(sqlQuery, connection)
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        If reader.HasRows Then
                            ' The query has a result
                            MsgBox("TIME CONFLICT!!!") 'it has conflict in start time
                            connection.Close()
                            bool = True
                        Else
                            connection.Close()
                            Try
                                Dim sqlQuery1 As String = "SELECT * FROM schedule WHERE roomnumber = '" + ComboBox7.SelectedItem + "' AND day =  '" + ComboBox3.SelectedItem + "' AND  '" + TextBox2.Text + "'BETWEEn starttime and endtime" ' Replace with your MySQL query
                                Using connection1 As New MySqlConnection(connectionString)
                                    connection1.Open()
                                    Using command1 As New MySqlCommand(sqlQuery1, connection1)
                                        Using reader1 As MySqlDataReader = command1.ExecuteReader()
                                            If reader1.HasRows Then
                                                ' The query has a result
                                                MsgBox("TIME CONFLICT!!!") 'it has conflict in end time
                                                connection1.Close()
                                                bool = True
                                            Else
                                                connection1.Close()
                                                bool = False
                                            End If
                                        End Using
                                    End Using
                                End Using
                            Catch ex As Exception
                                MsgBox(ex.Message)
                            End Try

                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        If bool = False Then
            'submiting schedule
            Using connection As New MySqlConnection(connectionString)
                connection.Open()
                Dim query As String = "INSERT INTO schedule (yearlevel,section,roomnumber,instructor,day,starttime,endtime) VALUES (@year,@section,@roomnumber,@instructor,@day,@starttime,@endtime)" ' Adjust column names and query as per your table structure
                Using command As New MySqlCommand(query, connection)
                    command.Parameters.AddWithValue("@year", Label8.Text)
                    command.Parameters.AddWithValue("@section", ComboBox1.SelectedItem)
                    command.Parameters.AddWithValue("@instructor", ComboBox2.SelectedItem)
                    command.Parameters.AddWithValue("@roomnumber", ComboBox7.SelectedItem)
                    command.Parameters.AddWithValue("@day", ComboBox3.SelectedItem)
                    command.Parameters.AddWithValue("@starttime", stime)
                    command.Parameters.AddWithValue("@endtime", etime)
                    MsgBox("schedule inserted")
                    command.ExecuteNonQuery()
                End Using
                connection.Close()
            End Using

            'clearing comboboxes and textboxes
            ComboBox1.SelectedIndex = -1
            ComboBox2.SelectedIndex = -1
            ComboBox3.SelectedIndex = -1
            ComboBox7.SelectedIndex = -1
ending:
            TextBox1.Clear()
            TextBox2.Clear()
            Call quee()
        End If

    End Sub

    'search all data to store in listview (search Function)
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ComboBox4.SelectedIndex = -1
        ComboBox8.SelectedIndex = -1
        ComboBox5.SelectedIndex = -1
        ComboBox6.SelectedIndex = -1
        Call quee()

    End Sub

    'Close scheduler, go back to front page
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Form1.Show()
        Me.Close()
    End Sub

    'Searching function by section
    Private Sub ComboBox5_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox5.SelectedIndexChanged
        If ComboBox4.SelectedIndex <> -1 And ComboBox8.SelectedIndex <> -1 And ComboBox6.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   roomnumber = '" + ComboBox8.SelectedItem + "' and day = '" + ComboBox4.SelectedItem + "' and instructor = '" + ComboBox6.SelectedItem + "' and section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox4.SelectedIndex <> -1 And ComboBox6.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where  day = '" + ComboBox4.SelectedItem + "' and instructor = '" + ComboBox6.SelectedItem + "' and section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox4.SelectedIndex <> -1 And ComboBox8.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   roomnumber = '" + ComboBox8.SelectedItem + "' and day = '" + ComboBox4.SelectedItem + "' and  section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox8.SelectedIndex <> -1 And ComboBox6.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   roomnumber = '" + ComboBox8.SelectedItem + "' and  instructor = '" + ComboBox6.SelectedItem + "' and section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox4.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where  day = '" + ComboBox4.SelectedItem + "' and section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox8.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   roomnumber = '" + ComboBox8.SelectedItem + "' and section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox6.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   instructor = '" + ComboBox6.SelectedItem + "' and section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "
        Else
            sql = "SELECT * from schedule where section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "
        End If

        Try
            ListView1.Items.Clear()
            conn = New MySqlConnection(cnstr)
            conn.Open()
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

    'Searching function by instructor
    Private Sub ComboBox6_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox6.SelectedIndexChanged
        If ComboBox4.SelectedIndex <> -1 And ComboBox8.SelectedIndex <> -1 And ComboBox5.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   roomnumber = '" + ComboBox8.SelectedItem + "' and day = '" + ComboBox4.SelectedItem + "' and instructor = '" + ComboBox6.SelectedItem + "' and section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox4.SelectedIndex <> -1 And ComboBox5.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where  day = '" + ComboBox4.SelectedItem + "' and instructor = '" + ComboBox6.SelectedItem + "' and section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox4.SelectedIndex <> -1 And ComboBox8.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   roomnumber = '" + ComboBox8.SelectedItem + "' and day = '" + ComboBox4.SelectedItem + "' and  instructor = '" + ComboBox6.SelectedItem + "' and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox8.SelectedIndex <> -1 And ComboBox5.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   roomnumber = '" + ComboBox8.SelectedItem + "' and  instructor = '" + ComboBox6.SelectedItem + "' and section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox4.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where  day = '" + ComboBox4.SelectedItem + "' and instructor = '" + ComboBox6.SelectedItem + "' and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox8.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   roomnumber = '" + ComboBox8.SelectedItem + "' and instructor = '" + ComboBox6.SelectedItem + "' and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox5.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   instructor = '" + ComboBox6.SelectedItem + "' and section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "
        Else
            sql = "SELECT * from schedule where instructor = '" + ComboBox6.SelectedItem + "' and yearlevel = '" + Label8.Text + "' "
        End If
        Try
            ListView1.Items.Clear()
            conn = New MySqlConnection(cnstr)
            conn.Open()
            'Dim sql As String = "SELECT * from schedule where instructor = '" + ComboBox6.SelectedItem + "'  and year = '" + Label8.Text + "' "
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

    'Searching function by day
    Private Sub ComboBox4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox4.SelectedIndexChanged
        If ComboBox8.SelectedIndex <> -1 And ComboBox6.SelectedIndex <> -1 And ComboBox5.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   roomnumber = '" + ComboBox8.SelectedItem + "' and day = '" + ComboBox4.SelectedItem + "' and instructor = '" + ComboBox6.SelectedItem + "' and section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox8.SelectedIndex <> -1 And ComboBox5.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where  day = '" + ComboBox4.SelectedItem + "' and roomnumber = '" + ComboBox8.SelectedItem + "' and section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox8.SelectedIndex <> -1 And ComboBox6.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   roomnumber = '" + ComboBox8.SelectedItem + "' and day = '" + ComboBox4.SelectedItem + "' and  instructor = '" + ComboBox6.SelectedItem + "' and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox6.SelectedIndex <> -1 And ComboBox5.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   day = '" + ComboBox4.SelectedItem + "' and  instructor = '" + ComboBox6.SelectedItem + "' and section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox5.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where  day = '" + ComboBox4.SelectedItem + "' and section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox8.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   day = '" + ComboBox4.SelectedItem + "' and roomnumber = '" + ComboBox8.SelectedItem + "' and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox6.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   instructor = '" + ComboBox6.SelectedItem + "' and day = '" + ComboBox4.SelectedItem + "' and yearlevel = '" + Label8.Text + "' "
        Else
            sql = "SELECT * from schedule where day = '" + ComboBox4.SelectedItem + "' and yearlevel = '" + Label8.Text + "' "
        End If
        Try
            ListView1.Items.Clear()
            conn = New MySqlConnection(cnstr)
            conn.Open()
            'Dim sql As String = "SELECT * from schedule where day = '" + ComboBox4.SelectedItem + "'  and year = '" + Label8.Text + "' "
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

    'Searching function by roomnumber
    Private Sub ComboBox8_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox8.SelectedIndexChanged
        If ComboBox4.SelectedIndex <> -1 And ComboBox6.SelectedIndex <> -1 And ComboBox5.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   roomnumber = '" + ComboBox8.SelectedItem + "' and day = '" + ComboBox4.SelectedItem + "' and instructor = '" + ComboBox6.SelectedItem + "' and section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "

        ElseIf ComboBox4.SelectedIndex <> -1 And ComboBox5.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where  day = '" + ComboBox4.SelectedItem + "' and roomnumber = '" + ComboBox8.SelectedItem + "' and section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox4.SelectedIndex <> -1 And ComboBox6.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   roomnumber = '" + ComboBox8.SelectedItem + "' and day = '" + ComboBox4.SelectedItem + "' and  instructor = '" + ComboBox6.SelectedItem + "' and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox6.SelectedIndex <> -1 And ComboBox5.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   roomnumber = '" + ComboBox8.SelectedItem + "' and  instructor = '" + ComboBox6.SelectedItem + "' and section = '" + ComboBox5.SelectedItem + "'  and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox4.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where  day = '" + ComboBox4.SelectedItem + "' and roomnumber = '" + ComboBox8.SelectedItem + "' and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox6.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   roomnumber = '" + ComboBox8.SelectedItem + "' and roomnumber = '" + ComboBox8.SelectedItem + "' and yearlevel = '" + Label8.Text + "' "
        ElseIf ComboBox6.SelectedIndex <> -1 Then
            sql = "SELECT * from schedule where   instructor = '" + ComboBox6.SelectedItem + "' roomnumber = '" + ComboBox8.SelectedItem + "' and yearlevel = '" + Label8.Text + "' "
        Else
            sql = "SELECT * from schedule where roomnumber = '" + ComboBox8.SelectedItem + "' and yearlevel = '" + Label8.Text + "' "
        End If
        Try
            ListView1.Items.Clear()
            conn = New MySqlConnection(cnstr)
            conn.Open()
            'Dim sql As String = "SELECT * from schedule where roomnumber = '" + ComboBox8.SelectedItem + "'  and year = '" + Label8.Text + "' "
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

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Me.Close()
        Deleteschedule.Show()
    End Sub
    Dim integ As Integer = 0
    Dim schedulerid As Integer
    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count > 0 Then
            integ = 1
            schedulerid = ListView1.Items(ListView1.SelectedIndices(0)).Text
            TextBox2.Text = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(1).Text
            ComboBox1.SelectedItem = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(2).Text
            ComboBox7.SelectedItem = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(3).Text
            ComboBox2.SelectedItem = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(4).Text
            ComboBox3.SelectedItem = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(5).Text
            TextBox1.Text = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(6).Text
            TextBox2.Text = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(7).Text
        End If
    End Sub
    Dim tempstime As String
    Dim tempetime As String
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim result = MessageBox.Show("Are you sure you want to update the data?", "Confirmation", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            Dim connectionString As String = "server=localhost; User=root; password=; database=class_scheduling_system" ' connection string

            Dim tableName As String = "Schedules" ' table name
            Dim timeString As String = TextBox1.Text.Replace(" ", "")
            Dim format As String = "h:mmtt"  ' Format for 12-hour clock with AM/PM indicator
            Dim parsedTime As DateTime
            If DateTime.TryParseExact(timeString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, parsedTime) Then
                Dim dateTimeValue As DateTime = parsedTime
                Dim timeValue As TimeSpan = dateTimeValue.TimeOfDay
                stime = timeValue.ToString
                'MsgBox(timeValue.ToString)
            Else
                'MsgBox("Invalid time format")
            End If
            If DateTime.TryParseExact(TextBox2.Text.Replace(" ", ""), format, CultureInfo.InvariantCulture, DateTimeStyles.None, parsedTime) Then
                Dim dateTimeValue As DateTime = parsedTime
                Dim timeValue As TimeSpan = dateTimeValue.TimeOfDay
                etime = timeValue.ToString
                'MsgBox(timeValue.ToString)
            Else
                MsgBox("Invalid time format")
            End If
            Dim bool As Boolean
            'finding if there is conflict in schedules
            Try
                Dim sqlQuery As String = "SELECT * FROM schedule WHERE instructor = '" + ComboBox2.SelectedItem + "' and roomnumber = '" + ComboBox7.SelectedItem + "' AND day =  '" + ComboBox3.SelectedItem + "' AND  '" + TextBox1.Text + "'BETWEEN starttime and endtime" ' Replace with your MySQL query
                Using connection As New MySqlConnection(connectionString)
                    connection.Open()
                    Using command As New MySqlCommand(sqlQuery, connection)
                        Using reader As MySqlDataReader = command.ExecuteReader()
                            If reader.HasRows Then
                                ' The query has a result
                                MsgBox("TIME CONFLICT!!!") 'it has conflict in start time
                                connection.Close()
                                bool = True
                            Else
                                connection.Close()
                                Try
                                    Dim sqlQuery1 As String = "SELECT * FROM schedule WHERE roomnumber = '" + ComboBox7.SelectedItem + "' AND day =  '" + ComboBox3.SelectedItem + "' AND  '" + TextBox2.Text + "'BETWEEN starttime and endtime" ' Replace with your MySQL query
                                    Using connection1 As New MySqlConnection(connectionString)
                                        connection1.Open()
                                        Using command1 As New MySqlCommand(sqlQuery1, connection1)
                                            Using reader1 As MySqlDataReader = command1.ExecuteReader()
                                                If reader1.HasRows Then
                                                    ' The query has a result
                                                    MsgBox("TIME CONFLICT!!!") 'it has conflict in end time
                                                    connection1.Close()
                                                    bool = True
                                                Else
                                                    connection1.Close()
                                                    bool = False
                                                End If
                                            End Using
                                        End Using
                                    End Using
                                Catch ex As Exception
                                    MsgBox(ex.Message)
                                End Try

                            End If
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            If bool = False Then
                Try

                Catch ex As Exception

                End Try
                'submiting schedule
                Using connection As New MySqlConnection(connectionString)
                    connection.Open()
                    MsgBox(Label8.Text)

                    Dim query As String = "UPDATE schedule SET yearlevel = @yearlevel,section = @section ,roomnumber = @roomnumber,instructor = @instructor,day = @day,starttime = @starttime,endtime = @endtime WHERE scheduleid = '" + schedulerid.ToString + "' " ' Adjust column names and query as per your table structure
                    Using command As New MySqlCommand(query, connection)
                        command.Parameters.AddWithValue("@yearlevel", Label8.Text)
                        command.Parameters.AddWithValue("@section", ComboBox1.SelectedItem)
                        command.Parameters.AddWithValue("@instructor", ComboBox2.SelectedItem)
                        command.Parameters.AddWithValue("@roomnumber", ComboBox7.SelectedItem)
                        command.Parameters.AddWithValue("@day", ComboBox3.SelectedItem)
                        command.Parameters.AddWithValue("@starttime", stime)
                        command.Parameters.AddWithValue("@endtime", etime)
                        MsgBox("Schedule Updated")
                        command.ExecuteNonQuery()
                    End Using
                    connection.Close()
                End Using
                'clearing comboboxes and textboxes
                ComboBox1.SelectedIndex = -1
                ComboBox2.SelectedIndex = -1
                ComboBox3.SelectedIndex = -1
                ComboBox7.SelectedIndex = -1
                TextBox1.Clear()
                TextBox2.Clear()
                Call quee()
            End If

        Else
            MsgBox("Update Data Cancelled")
        End If
    End Sub

    Private Sub TextBox2_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            e.Handled = True ' Prevents the Enter key from being entered in the textbox
            Button1.PerformClick() ' Trigger the button's click event
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        ComboBox4.SelectedIndex = -1
        ComboBox8.SelectedIndex = -1
        ComboBox5.SelectedIndex = -1
        ComboBox6.SelectedIndex = -1
        Call quee()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If integ = 1 Then

        Else
            Try
                ListView1.Items.Clear()
                conn = New MySqlConnection(cnstr)
                conn.Open()
                Dim sql As String = "SELECT scheduleid,yearlevel,section,roomnumber,instructor,day,TIME_FORMAT(starttime, '%h:%i %p') AS starttime,TIME_FORMAT(endtime, '%h:%i %p') AS endtime from schedule where yearlevel = '" + Label8.Text + "' AND section = '" + ComboBox1.SelectedItem + "'"

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
        End If
        
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        If integ = 1 Then

        Else
            Try
                ListView1.Items.Clear()
                conn = New MySqlConnection(cnstr)
                conn.Open()
                Dim sql As String = "SELECT scheduleid,yearlevel,section,roomnumber,instructor,day,TIME_FORMAT(starttime, '%h:%i %p') AS starttime,TIME_FORMAT(endtime, '%h:%i %p') AS endtime from schedule where yearlevel = '" + Label8.Text + "' AND section='" + ComboBox1.SelectedItem + "'AND instructor = '" + ComboBox2.SelectedItem + "' "

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
        End If
        
    End Sub

    Private Sub ComboBox7_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox7.SelectedIndexChanged
      
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        
    End Sub
End Class