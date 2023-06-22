Imports MySql.Data.MySqlClient
Public Class Form1
    Dim cnstr As String = "server=localhost; User=root; password=; database=class_scheduling_system" 'connection string
    Dim sql As String 'variable for storing queries
    Dim conn As New MySqlConnection(cnstr)
    Dim cmd As MySqlCommand

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Hide()
        scheduler.Label8.Text = "1st Year"
        scheduler.Show()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Me.Hide()
        Add_Section.Show()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Hide()
        scheduler.Label8.Text = "2nd Year"
        scheduler.Show()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Hide()
        scheduler.Label8.Text = "3rd Year"
        scheduler.Show()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Hide()
        scheduler.Label8.Text = "4th Year"
        scheduler.Show()
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
        scheduler.Label8.Text = "5th Year"
        scheduler.Show()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Me.Hide()
        Add_Instructor.Show()
    End Sub

   
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
