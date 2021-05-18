namespace GangTexts
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._readButton = new System.Windows.Forms.Button();
            this._filePathTextBox = new System.Windows.Forms.TextBox();
            this._germanTextRichTextBox = new System.Windows.Forms.RichTextBox();
            this._englishTextRichTextBox = new System.Windows.Forms.RichTextBox();
            this._browseButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._saveButton = new System.Windows.Forms.Button();
            this._closeButton = new System.Windows.Forms.Button();
            this._addPairButton = new System.Windows.Forms.Button();
            this._newKeyTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._newActionTextBox = new System.Windows.Forms.TextBox();
            this._addToExistingActionButton = new System.Windows.Forms.Button();
            this._addTextPairButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this._textPairTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this._currentKeyTextBox = new System.Windows.Forms.TextBox();
            this._updateButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this._newItemKeyTextBox = new System.Windows.Forms.TextBox();
            this._addItemPackageButton = new System.Windows.Forms.Button();
            this._handleTextRessourceRadioButton = new System.Windows.Forms.RadioButton();
            this._handleItemRessourceRadioButton = new System.Windows.Forms.RadioButton();
            this._generateKeyButton = new System.Windows.Forms.Button();
            this._keyListBox = new System.Windows.Forms.ListBox();
            this._childrenListBox = new System.Windows.Forms.ListBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _readButton
            // 
            this._readButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._readButton.Location = new System.Drawing.Point(993, 12);
            this._readButton.Name = "_readButton";
            this._readButton.Size = new System.Drawing.Size(75, 23);
            this._readButton.TabIndex = 1;
            this._readButton.Text = "Read";
            this._readButton.UseVisualStyleBackColor = true;
            this._readButton.Click += new System.EventHandler(this._readButton_Click);
            // 
            // _filePathTextBox
            // 
            this._filePathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._filePathTextBox.Location = new System.Drawing.Point(12, 12);
            this._filePathTextBox.Name = "_filePathTextBox";
            this._filePathTextBox.Size = new System.Drawing.Size(942, 20);
            this._filePathTextBox.TabIndex = 2;
            this._filePathTextBox.Text = "D:\\Unity Projects\\GangCards\\Assets\\Resources\\TextResources.txt";
            // 
            // _germanTextRichTextBox
            // 
            this._germanTextRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._germanTextRichTextBox.Location = new System.Drawing.Point(3, 23);
            this._germanTextRichTextBox.Name = "_germanTextRichTextBox";
            this._germanTextRichTextBox.Size = new System.Drawing.Size(411, 221);
            this._germanTextRichTextBox.TabIndex = 5;
            this._germanTextRichTextBox.Text = "";
            this._germanTextRichTextBox.TextChanged += new System.EventHandler(this._germanTextRichTextBox_TextChanged);
            // 
            // _englishTextRichTextBox
            // 
            this._englishTextRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._englishTextRichTextBox.Location = new System.Drawing.Point(420, 23);
            this._englishTextRichTextBox.Name = "_englishTextRichTextBox";
            this._englishTextRichTextBox.Size = new System.Drawing.Size(412, 221);
            this._englishTextRichTextBox.TabIndex = 6;
            this._englishTextRichTextBox.Text = "";
            this._englishTextRichTextBox.TextChanged += new System.EventHandler(this._englishTextRichTextBox_TextChanged);
            // 
            // _browseButton
            // 
            this._browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._browseButton.Location = new System.Drawing.Point(960, 12);
            this._browseButton.Name = "_browseButton";
            this._browseButton.Size = new System.Drawing.Size(27, 23);
            this._browseButton.TabIndex = 7;
            this._browseButton.Text = "...";
            this._browseButton.UseVisualStyleBackColor = true;
            this._browseButton.Click += new System.EventHandler(this._browseButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Deutsch";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(420, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Englisch";
            // 
            // _saveButton
            // 
            this._saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._saveButton.Location = new System.Drawing.Point(1070, 12);
            this._saveButton.Name = "_saveButton";
            this._saveButton.Size = new System.Drawing.Size(75, 23);
            this._saveButton.TabIndex = 10;
            this._saveButton.Text = "Write";
            this._saveButton.UseVisualStyleBackColor = true;
            this._saveButton.Click += new System.EventHandler(this._saveButton_Click);
            // 
            // _closeButton
            // 
            this._closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._closeButton.Location = new System.Drawing.Point(1066, 444);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(75, 23);
            this._closeButton.TabIndex = 11;
            this._closeButton.Text = "Close";
            this._closeButton.UseVisualStyleBackColor = true;
            this._closeButton.Click += new System.EventHandler(this._closeButton_Click);
            // 
            // _addPairButton
            // 
            this._addPairButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._addPairButton.Location = new System.Drawing.Point(1066, 391);
            this._addPairButton.Name = "_addPairButton";
            this._addPairButton.Size = new System.Drawing.Size(75, 23);
            this._addPairButton.TabIndex = 12;
            this._addPairButton.Text = "Add";
            this._addPairButton.UseVisualStyleBackColor = true;
            this._addPairButton.Click += new System.EventHandler(this._addButton_Click);
            // 
            // _newKeyTextBox
            // 
            this._newKeyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._newKeyTextBox.Location = new System.Drawing.Point(424, 393);
            this._newKeyTextBox.Name = "_newKeyTextBox";
            this._newKeyTextBox.Size = new System.Drawing.Size(636, 20);
            this._newKeyTextBox.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(306, 397);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Add Building Container:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(306, 347);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "New Action to existing:";
            // 
            // _newActionTextBox
            // 
            this._newActionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._newActionTextBox.Location = new System.Drawing.Point(424, 345);
            this._newActionTextBox.Name = "_newActionTextBox";
            this._newActionTextBox.Size = new System.Drawing.Size(636, 20);
            this._newActionTextBox.TabIndex = 15;
            // 
            // _addToExistingActionButton
            // 
            this._addToExistingActionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._addToExistingActionButton.Location = new System.Drawing.Point(1066, 342);
            this._addToExistingActionButton.Name = "_addToExistingActionButton";
            this._addToExistingActionButton.Size = new System.Drawing.Size(75, 23);
            this._addToExistingActionButton.TabIndex = 17;
            this._addToExistingActionButton.Text = "Add";
            this._addToExistingActionButton.UseVisualStyleBackColor = true;
            this._addToExistingActionButton.Click += new System.EventHandler(this._addToExistingActionButton_Click);
            // 
            // _addTextPairButton
            // 
            this._addTextPairButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._addTextPairButton.Location = new System.Drawing.Point(1066, 366);
            this._addTextPairButton.Name = "_addTextPairButton";
            this._addTextPairButton.Size = new System.Drawing.Size(75, 23);
            this._addTextPairButton.TabIndex = 20;
            this._addTextPairButton.Text = "Add";
            this._addTextPairButton.UseVisualStyleBackColor = true;
            this._addTextPairButton.Click += new System.EventHandler(this._addTextPairButton_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(306, 372);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Add Action Container:";
            // 
            // _textPairTextBox
            // 
            this._textPairTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textPairTextBox.Location = new System.Drawing.Point(424, 369);
            this._textPairTextBox.Name = "_textPairTextBox";
            this._textPairTextBox.Size = new System.Drawing.Size(636, 20);
            this._textPairTextBox.TabIndex = 18;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this._englishTextRichTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._germanTextRichTextBox, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(306, 62);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(835, 247);
            this.tableLayoutPanel1.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(306, 322);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Current Key";
            // 
            // _currentKeyTextBox
            // 
            this._currentKeyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._currentKeyTextBox.Location = new System.Drawing.Point(424, 320);
            this._currentKeyTextBox.Name = "_currentKeyTextBox";
            this._currentKeyTextBox.Size = new System.Drawing.Size(636, 20);
            this._currentKeyTextBox.TabIndex = 22;
            // 
            // _updateButton
            // 
            this._updateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._updateButton.Location = new System.Drawing.Point(1066, 318);
            this._updateButton.Name = "_updateButton";
            this._updateButton.Size = new System.Drawing.Size(75, 23);
            this._updateButton.TabIndex = 24;
            this._updateButton.Text = "Update";
            this._updateButton.UseVisualStyleBackColor = true;
            this._updateButton.Click += new System.EventHandler(this._updateButton_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(306, 421);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "New Item Package";
            // 
            // _newItemKeyTextBox
            // 
            this._newItemKeyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._newItemKeyTextBox.Location = new System.Drawing.Point(424, 417);
            this._newItemKeyTextBox.Name = "_newItemKeyTextBox";
            this._newItemKeyTextBox.Size = new System.Drawing.Size(555, 20);
            this._newItemKeyTextBox.TabIndex = 26;
            // 
            // _addItemPackageButton
            // 
            this._addItemPackageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._addItemPackageButton.Location = new System.Drawing.Point(985, 414);
            this._addItemPackageButton.Name = "_addItemPackageButton";
            this._addItemPackageButton.Size = new System.Drawing.Size(75, 23);
            this._addItemPackageButton.TabIndex = 25;
            this._addItemPackageButton.Text = "Add";
            this._addItemPackageButton.UseVisualStyleBackColor = true;
            this._addItemPackageButton.Click += new System.EventHandler(this._addItemPackageButton_Click);
            // 
            // _handleTextRessourceRadioButton
            // 
            this._handleTextRessourceRadioButton.AutoSize = true;
            this._handleTextRessourceRadioButton.Checked = true;
            this._handleTextRessourceRadioButton.Location = new System.Drawing.Point(12, 38);
            this._handleTextRessourceRadioButton.Name = "_handleTextRessourceRadioButton";
            this._handleTextRessourceRadioButton.Size = new System.Drawing.Size(142, 17);
            this._handleTextRessourceRadioButton.TabIndex = 28;
            this._handleTextRessourceRadioButton.TabStop = true;
            this._handleTextRessourceRadioButton.Text = "Handle Text Ressources";
            this._handleTextRessourceRadioButton.UseVisualStyleBackColor = true;
            this._handleTextRessourceRadioButton.CheckedChanged += new System.EventHandler(this._handleTextRessourceRadioButton_CheckedChanged);
            // 
            // _handleItemRessourceRadioButton
            // 
            this._handleItemRessourceRadioButton.AutoSize = true;
            this._handleItemRessourceRadioButton.Location = new System.Drawing.Point(160, 38);
            this._handleItemRessourceRadioButton.Name = "_handleItemRessourceRadioButton";
            this._handleItemRessourceRadioButton.Size = new System.Drawing.Size(141, 17);
            this._handleItemRessourceRadioButton.TabIndex = 29;
            this._handleItemRessourceRadioButton.Text = "Handle Item Ressources";
            this._handleItemRessourceRadioButton.UseVisualStyleBackColor = true;
            this._handleItemRessourceRadioButton.CheckedChanged += new System.EventHandler(this._handleTextRessourceRadioButton_CheckedChanged);
            // 
            // _generateKeyButton
            // 
            this._generateKeyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._generateKeyButton.Location = new System.Drawing.Point(1066, 415);
            this._generateKeyButton.Name = "_generateKeyButton";
            this._generateKeyButton.Size = new System.Drawing.Size(75, 23);
            this._generateKeyButton.TabIndex = 30;
            this._generateKeyButton.Text = "Generate";
            this._generateKeyButton.UseVisualStyleBackColor = true;
            this._generateKeyButton.Click += new System.EventHandler(this._generateKeyButton_Click);
            // 
            // _keyListBox
            // 
            this._keyListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._keyListBox.FormattingEnabled = true;
            this._keyListBox.Location = new System.Drawing.Point(3, 23);
            this._keyListBox.Name = "_keyListBox";
            this._keyListBox.Size = new System.Drawing.Size(282, 173);
            this._keyListBox.TabIndex = 31;
            this._keyListBox.SelectedIndexChanged += new System.EventHandler(this._keyListBox_SelectedIndexChanged);
            // 
            // _childrenListBox
            // 
            this._childrenListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._childrenListBox.FormattingEnabled = true;
            this._childrenListBox.Location = new System.Drawing.Point(3, 224);
            this._childrenListBox.Name = "_childrenListBox";
            this._childrenListBox.Size = new System.Drawing.Size(282, 173);
            this._childrenListBox.TabIndex = 32;
            this._childrenListBox.SelectedIndexChanged += new System.EventHandler(this._entriesListBox_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 208);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "Children:";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 7);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 13);
            this.label10.TabIndex = 35;
            this.label10.Text = "Parents:";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this._childrenListBox, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this._keyListBox, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(12, 62);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(288, 402);
            this.tableLayoutPanel2.TabIndex = 36;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1153, 476);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this._generateKeyButton);
            this.Controls.Add(this._handleItemRessourceRadioButton);
            this.Controls.Add(this._handleTextRessourceRadioButton);
            this.Controls.Add(this.label7);
            this.Controls.Add(this._newItemKeyTextBox);
            this.Controls.Add(this._addItemPackageButton);
            this.Controls.Add(this._updateButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this._currentKeyTextBox);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this._addTextPairButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this._textPairTextBox);
            this.Controls.Add(this._addToExistingActionButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._newActionTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._newKeyTextBox);
            this.Controls.Add(this._addPairButton);
            this.Controls.Add(this._closeButton);
            this.Controls.Add(this._saveButton);
            this.Controls.Add(this._browseButton);
            this.Controls.Add(this._filePathTextBox);
            this.Controls.Add(this._readButton);
            this.MinimumSize = new System.Drawing.Size(1169, 458);
            this.Name = "MainForm";
            this.Text = "Ressource Maintainence";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button _readButton;
        private System.Windows.Forms.TextBox _filePathTextBox;
        private System.Windows.Forms.RichTextBox _germanTextRichTextBox;
        private System.Windows.Forms.RichTextBox _englishTextRichTextBox;
        private System.Windows.Forms.Button _browseButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button _saveButton;
        private System.Windows.Forms.Button _closeButton;
        private System.Windows.Forms.Button _addPairButton;
        private System.Windows.Forms.TextBox _newKeyTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox _newActionTextBox;
        private System.Windows.Forms.Button _addToExistingActionButton;
        private System.Windows.Forms.Button _addTextPairButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox _textPairTextBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox _currentKeyTextBox;
        private System.Windows.Forms.Button _updateButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox _newItemKeyTextBox;
        private System.Windows.Forms.Button _addItemPackageButton;
        private System.Windows.Forms.RadioButton _handleTextRessourceRadioButton;
        private System.Windows.Forms.RadioButton _handleItemRessourceRadioButton;
        private System.Windows.Forms.Button _generateKeyButton;
        private System.Windows.Forms.ListBox _keyListBox;
        private System.Windows.Forms.ListBox _childrenListBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}

