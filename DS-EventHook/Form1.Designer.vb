<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnHook = New System.Windows.Forms.Button()
        Me.dgvEvents = New System.Windows.Forms.DataGridView()
        Me.nmbMaxFlags = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tabs = New System.Windows.Forms.TabControl()
        Me.tabLog = New System.Windows.Forms.TabPage()
        Me.tabIDs = New System.Windows.Forms.TabPage()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.btnSet = New System.Windows.Forms.Button()
        Me.txtValue = New System.Windows.Forms.TextBox()
        Me.btnGet = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.dgvNames = New System.Windows.Forms.DataGridView()
        Me.btnUnhook = New System.Windows.Forms.Button()
        Me.lblVer = New System.Windows.Forms.Label()
        Me.btnUpdate = New System.Windows.Forms.Button()
        CType(Me.dgvEvents,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.nmbMaxFlags,System.ComponentModel.ISupportInitialize).BeginInit
        Me.tabs.SuspendLayout
        Me.tabLog.SuspendLayout
        Me.tabIDs.SuspendLayout
        CType(Me.dgvNames,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        'btnHook
        '
        Me.btnHook.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.btnHook.Location = New System.Drawing.Point(16, 507)
        Me.btnHook.Name = "btnHook"
        Me.btnHook.Size = New System.Drawing.Size(75, 23)
        Me.btnHook.TabIndex = 0
        Me.btnHook.Text = "Hook"
        Me.btnHook.UseVisualStyleBackColor = true
        '
        'dgvEvents
        '
        Me.dgvEvents.AllowUserToAddRows = false
        Me.dgvEvents.AllowUserToResizeRows = false
        Me.dgvEvents.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.dgvEvents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEvents.Location = New System.Drawing.Point(6, 6)
        Me.dgvEvents.Name = "dgvEvents"
        Me.dgvEvents.Size = New System.Drawing.Size(472, 425)
        Me.dgvEvents.TabIndex = 1
        '
        'nmbMaxFlags
        '
        Me.nmbMaxFlags.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.nmbMaxFlags.Location = New System.Drawing.Point(431, 437)
        Me.nmbMaxFlags.Name = "nmbMaxFlags"
        Me.nmbMaxFlags.Size = New System.Drawing.Size(47, 20)
        Me.nmbMaxFlags.TabIndex = 2
        Me.nmbMaxFlags.Value = New Decimal(New Integer() {18, 0, 0, 0})
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = true
        Me.Label1.Location = New System.Drawing.Point(395, 439)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(30, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Max:"
        '
        'tabs
        '
        Me.tabs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.tabs.Controls.Add(Me.tabLog)
        Me.tabs.Controls.Add(Me.tabIDs)
        Me.tabs.Location = New System.Drawing.Point(12, 12)
        Me.tabs.Name = "tabs"
        Me.tabs.SelectedIndex = 0
        Me.tabs.Size = New System.Drawing.Size(492, 489)
        Me.tabs.TabIndex = 4
        '
        'tabLog
        '
        Me.tabLog.BackColor = System.Drawing.SystemColors.Control
        Me.tabLog.Controls.Add(Me.dgvEvents)
        Me.tabLog.Controls.Add(Me.Label1)
        Me.tabLog.Controls.Add(Me.nmbMaxFlags)
        Me.tabLog.Location = New System.Drawing.Point(4, 22)
        Me.tabLog.Name = "tabLog"
        Me.tabLog.Padding = New System.Windows.Forms.Padding(3)
        Me.tabLog.Size = New System.Drawing.Size(484, 463)
        Me.tabLog.TabIndex = 0
        Me.tabLog.Text = "Log"
        '
        'tabIDs
        '
        Me.tabIDs.BackColor = System.Drawing.SystemColors.Control
        Me.tabIDs.Controls.Add(Me.btnSave)
        Me.tabIDs.Controls.Add(Me.btnLoad)
        Me.tabIDs.Controls.Add(Me.btnSet)
        Me.tabIDs.Controls.Add(Me.txtValue)
        Me.tabIDs.Controls.Add(Me.btnGet)
        Me.tabIDs.Controls.Add(Me.Label2)
        Me.tabIDs.Controls.Add(Me.txtID)
        Me.tabIDs.Controls.Add(Me.dgvNames)
        Me.tabIDs.Location = New System.Drawing.Point(4, 22)
        Me.tabIDs.Name = "tabIDs"
        Me.tabIDs.Padding = New System.Windows.Forms.Padding(3)
        Me.tabIDs.Size = New System.Drawing.Size(484, 463)
        Me.tabIDs.TabIndex = 1
        Me.tabIDs.Text = "IDs"
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(258, 434)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(220, 23)
        Me.btnSave.TabIndex = 16
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = true
        '
        'btnLoad
        '
        Me.btnLoad.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnLoad.Location = New System.Drawing.Point(6, 434)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(246, 23)
        Me.btnLoad.TabIndex = 15
        Me.btnLoad.Text = "Load DS-EventHook-IDs.txt"
        Me.btnLoad.UseVisualStyleBackColor = true
        '
        'btnSet
        '
        Me.btnSet.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnSet.Location = New System.Drawing.Point(403, 406)
        Me.btnSet.Name = "btnSet"
        Me.btnSet.Size = New System.Drawing.Size(75, 23)
        Me.btnSet.TabIndex = 14
        Me.btnSet.Text = "Set"
        Me.btnSet.UseVisualStyleBackColor = true
        '
        'txtValue
        '
        Me.txtValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtValue.Location = New System.Drawing.Point(261, 408)
        Me.txtValue.Name = "txtValue"
        Me.txtValue.Size = New System.Drawing.Size(58, 20)
        Me.txtValue.TabIndex = 12
        '
        'btnGet
        '
        Me.btnGet.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnGet.Location = New System.Drawing.Point(325, 406)
        Me.btnGet.Name = "btnGet"
        Me.btnGet.Size = New System.Drawing.Size(75, 23)
        Me.btnGet.TabIndex = 11
        Me.btnGet.Text = "Get"
        Me.btnGet.UseVisualStyleBackColor = true
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = true
        Me.Label2.Location = New System.Drawing.Point(5, 411)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Event Flag"
        '
        'txtID
        '
        Me.txtID.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtID.Location = New System.Drawing.Point(73, 408)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(182, 20)
        Me.txtID.TabIndex = 9
        '
        'dgvNames
        '
        Me.dgvNames.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.dgvNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvNames.Location = New System.Drawing.Point(6, 6)
        Me.dgvNames.Name = "dgvNames"
        Me.dgvNames.Size = New System.Drawing.Size(472, 396)
        Me.dgvNames.TabIndex = 2
        '
        'btnUnhook
        '
        Me.btnUnhook.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.btnUnhook.Location = New System.Drawing.Point(97, 507)
        Me.btnUnhook.Name = "btnUnhook"
        Me.btnUnhook.Size = New System.Drawing.Size(75, 23)
        Me.btnUnhook.TabIndex = 4
        Me.btnUnhook.Text = "Unhook"
        Me.btnUnhook.UseVisualStyleBackColor = true
        '
        'lblVer
        '
        Me.lblVer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.lblVer.AutoSize = true
        Me.lblVer.Location = New System.Drawing.Point(416, 512)
        Me.lblVer.Name = "lblVer"
        Me.lblVer.Size = New System.Drawing.Size(76, 13)
        Me.lblVer.TabIndex = 11
        Me.lblVer.Text = "2016.11.21.21"
        '
        'btnUpdate
        '
        Me.btnUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.btnUpdate.Location = New System.Drawing.Point(178, 507)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(182, 23)
        Me.btnUpdate.TabIndex = 77
        Me.btnUpdate.Text = "Update EventHook"
        Me.btnUpdate.UseVisualStyleBackColor = true
        Me.btnUpdate.Visible = false
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(512, 537)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.lblVer)
        Me.Controls.Add(Me.btnUnhook)
        Me.Controls.Add(Me.btnHook)
        Me.Controls.Add(Me.tabs)
        Me.Name = "Form1"
        Me.Text = "EventHook"
        CType(Me.dgvEvents,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.nmbMaxFlags,System.ComponentModel.ISupportInitialize).EndInit
        Me.tabs.ResumeLayout(false)
        Me.tabLog.ResumeLayout(false)
        Me.tabLog.PerformLayout
        Me.tabIDs.ResumeLayout(false)
        Me.tabIDs.PerformLayout
        CType(Me.dgvNames,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub

    Friend WithEvents btnHook As Button
    Friend WithEvents nmbMaxFlags As NumericUpDown
    Friend WithEvents Label1 As Label
    Friend WithEvents tabs As TabControl
    Friend WithEvents tabLog As TabPage
    Friend WithEvents tabIDs As TabPage
    Friend WithEvents btnSave As Button
    Friend WithEvents btnLoad As Button
    Friend WithEvents btnSet As Button
    Friend WithEvents txtValue As TextBox
    Friend WithEvents btnGet As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents txtID As TextBox
    Public WithEvents dgvEvents As DataGridView
    Public WithEvents dgvNames As DataGridView
    Friend WithEvents btnUnhook As Button
    Friend WithEvents lblVer As Label
    Friend WithEvents btnUpdate As Button
End Class
