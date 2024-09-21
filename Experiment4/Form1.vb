Imports System.IO.Ports
Imports MySql.Data.MySqlClient
Public Class Form1
    Dim WithEvents serialPort As New SerialPort()
    Dim connection As MySqlConnection
    Dim database_name As New String("experiment4")
    Dim table_name As New String("rotary")
    Dim cmd As MySqlCommand
    Dim data As New String("")

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connection = New MySqlConnection
        connection.ConnectionString = "server=localhost;username=root;password=;database='" & database_name & "'"
        serialPort.PortName = "COM3"
        serialPort.BaudRate = 9600
        serialPort.Parity = Parity.None
        serialPort.DataBits = 8
        serialPort.StopBits = StopBits.One
        serialPort.Handshake = Handshake.None
        serialPort.ReadTimeout = 500
        serialPort.WriteTimeout = 500
        Try
            serialPort.Open()
        Catch ex As Exception
            MsgBox("Failed to open serial port: " & ex.Message)
        End Try
    End Sub

    Private Sub serialPort_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles serialPort.DataReceived
        Try
            data = serialPort.ReadLine()
            Me.Invoke(Sub()
                          If data > "0" Then
                              Label5.Text = "STARTED"
                          End If
                          If data.StartsWith("S") Then
                              Label5.Text = data
                          ElseIf data.StartsWith("R") Then
                              Label5.Text = data
                              Label1.Text = "0"
                          Else
                              Label1.Text = data
                          End If
                      End Sub)
        Catch ex As TimeoutException
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        serialPort.Close()
    End Sub

    Private Sub ConnectionStatus_Click(sender As Object, e As EventArgs) Handles ConnectionStatus.Click
        Try
            If connection.State = ConnectionState.Closed Then
                connection.Open()
                ConnectionStatus.Text = "Connected"
                ConnectionStatus.BackColor = Color.Green
            Else
                connection.Close()
                ConnectionStatus.Text = "Disconnected"
                ConnectionStatus.BackColor = Color.Red
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            If connection.State = ConnectionState.Closed Then
                MsgBox("Please connect to the database first", vbExclamation, Title:="Warning")
            ElseIf Label5.Text = "Not Started" Then
                MsgBox("Please press the start buttkn", vbExclamation, Title:="Warning")
            Else
                If connection.State = ConnectionState.Closed Then
                    connection.Open()
                End If
                cmd = New MySqlCommand("INSERT INTO " & table_name & "(ROTARY_VAL, BUTTON_STATE) values (@ROTARY_VAL, @BUTTON_STATE)", connection)
                cmd.Parameters.AddWithValue("@ROTARY_VAL", Label1.Text)
                cmd.Parameters.AddWithValue("@BUTTON_STATE", Label5.Text)
                Dim i As Integer = cmd.ExecuteNonQuery
                If i > 0 Then
                    MsgBox("Successfully saved record", vbInformation, Title:="Success")
                Else
                    MsgBox("Failed to save record", vbExclamation, Title:="Warning")
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
