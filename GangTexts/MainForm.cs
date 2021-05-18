// ***********************************************************************
// Assembly         : GangTexts
// Author           : hendrik.jordt
// Created          : 03-17-2016
//
// Last Modified By : hendrik.jordt
// Last Modified On : 04-01-2016
// ***********************************************************************
// <copyright file="Form1.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GangTexts.Enums;

namespace GangTexts
{
    /// <summary>
    /// Class Form1.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// The _application scope
        /// </summary>
        private Scope _applicationScope;

        /// <summary>
        /// The _read data
        /// </summary>
        private Dictionary<string, string> _readData;

        /// <summary>
        /// The _english key
        /// </summary>
        private string _englishKey = String.Empty;

        /// <summary>
        /// The _germany key
        /// </summary>
        private string _germanyKey = String.Empty;

        /// <summary>
        /// The current key
        /// </summary>
        private string _currentKey = String.Empty;

        /// <summary>
        /// The data grouped by their respective keys.
        /// </summary>
        private readonly Dictionary<string, List<string>> _groupedData = new Dictionary<string, List<string>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm" /> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the _readButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _readButton_Click(object sender, EventArgs e)
        {
            ReadText();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the _entriesListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _entriesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var box = sender as ListBox;
            _currentKey = box.SelectedItem.ToString().Trim();

            if (_currentKey.ToUpperInvariant().EndsWith("_ENG"))
            {
                _englishKey = _currentKey;
                _germanyKey = _currentKey.Substring(0, _currentKey.Length - 4) + "_Ger";
            }
            else
            {
                _englishKey = _currentKey.Substring(0, _currentKey.Length - 4) + "_Eng";
                _germanyKey = _currentKey;
            }

            _germanTextRichTextBox.Text = _readData.ContainsKey(_germanyKey) ? _readData[_germanyKey] : String.Concat("Key ", _germanyKey, " not found!");
            _englishTextRichTextBox.Text = _readData.ContainsKey(_englishKey) ? _readData[_englishKey] : String.Concat("Key ", _englishKey, " not found!");

            _currentKeyTextBox.Text = _currentKey;

            _generateKeyButton.Enabled = _handleItemRessourceRadioButton.Checked && _germanyKey.StartsWith("Item_") && _germanyKey.EndsWith("Key");

            PresetFields();
        }

        /// <summary>
        /// Handles the Click event of the _browseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _browseButton_Click(object sender, EventArgs e)
        {
            FileDialog dia = new OpenFileDialog();
            dia.InitialDirectory = Environment.CurrentDirectory;
            dia.Filter = @"txt files (*.txt)|*.txt";

            if (dia.ShowDialog() == DialogResult.OK)
            {
                _filePathTextBox.Text = dia.FileName;
            }
        }

        /// <summary>
        /// Handles the Click event of the _closeButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _closeButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Handles the Click event of the _saveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _saveButton_Click(object sender, EventArgs e)
        {
            WriteText();

            ReadText();
        }

        /// <summary>
        /// Handles the TextChanged event of the _germanTextRichTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _germanTextRichTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_readData.ContainsKey(_germanyKey))
            {
                _readData[_germanyKey] = _germanTextRichTextBox.Text;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the _englishTextRichTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _englishTextRichTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_readData.ContainsKey(_englishKey))
            {
                _readData[_englishKey] = _englishTextRichTextBox.Text;
            }
        }

        /// <summary>
        /// Handles the Click event of the _addButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _addButton_Click(object sender, EventArgs e)
        {
            AddToData(String.Concat(_newKeyTextBox.Text, "Entry_Ger"), String.Concat(_newKeyTextBox.Text, "Entry_Eng"));
            AddToData(String.Concat(_newKeyTextBox.Text, "Action0_Ger"), String.Concat(_newKeyTextBox.Text, "Action0_Eng"));
        }

        /// <summary>
        /// Handles the Click event of the _addTextPairButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _addTextPairButton_Click(object sender, EventArgs e)
        {
            AddToData(String.Concat(_textPairTextBox.Text, "_Ger"), String.Concat(_textPairTextBox.Text, "_Eng"));
            AddToData(String.Concat(_textPairTextBox.Text, "Part1_Ger"), String.Concat(_textPairTextBox.Text, "Part1_Eng"));
            AddToData(String.Concat(_textPairTextBox.Text, "Part2_Ger"), String.Concat(_textPairTextBox.Text, "Part2_Eng"));
            AddToData(String.Concat(_textPairTextBox.Text, "Part1Fail_Ger"), String.Concat(_textPairTextBox.Text, "Part1Fail_Eng"));
            AddToData(String.Concat(_textPairTextBox.Text, "Part2Fail_Ger"), String.Concat(_textPairTextBox.Text, "Part2Fail_Eng"));
        }

        /// <summary>
        /// Handles the Click event of the _addToExistingActionButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _addToExistingActionButton_Click(object sender, EventArgs e)
        {
            string newKeyEng = String.Concat(_newActionTextBox.Text, "_Eng");
            string newKeyGer = String.Concat(_newActionTextBox.Text, "_Ger");

            AddToData(newKeyGer, newKeyEng);
        }

        /// <summary>
        /// Add new Item Package
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void _addItemPackageButton_Click(object sender, EventArgs e)
        {
            List<string> newKeysEng = new List<string>();
            newKeysEng.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Key"));
            newKeysEng.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Skill"));
            newKeysEng.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Rarity"));
            newKeysEng.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Level"));
            newKeysEng.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Name_Eng"));
            newKeysEng.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Type_Eng"));
            newKeysEng.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Slot_Eng"));
            newKeysEng.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Prop1Typ_Eng"));
            newKeysEng.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Prop2Typ_Eng"));
            newKeysEng.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Prop3Typ_Eng"));
            newKeysEng.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Prop1Val_Eng"));
            newKeysEng.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Prop2Val_Eng"));
            newKeysEng.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Prop3Val_Eng"));

            List<string> newKeysGer = new List<string>();
            newKeysGer.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Name_Ger"));
            newKeysGer.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Type_Ger"));
            newKeysEng.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Slot_Ger"));
            newKeysEng.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Prop1Typ_Ger"));
            newKeysEng.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Prop2Typ_Ger"));
            newKeysEng.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Prop3Typ_Ger"));
            newKeysGer.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Prop1Val_Ger"));
            newKeysGer.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Prop2Val_Ger"));
            newKeysGer.Add(String.Concat("Item_", _newItemKeyTextBox.Text, "Prop3Val_Ger"));

            AddToData(newKeysGer, newKeysEng);
            GroupData();
        }

        /// <summary>
        /// Presets the fields.
        /// </summary>
        private void PresetFields()
        {
            string nonLanguageKey = _germanyKey.Substring(0, _germanyKey.Length - 4);

            // Preset Action
            if (nonLanguageKey.Contains("Action"))
            {
                var index = nonLanguageKey.IndexOf("Action");
                var cutOff = nonLanguageKey.Substring(0, index);
                var startKey = String.Concat(cutOff, "Action");

                var pairs = _readData.Where(pair => pair.Key.StartsWith(startKey)).Select(pair => pair.Key).ToList();

                for (var i = 0; i < pairs.Count; i++)
                {
                    pairs[i] = pairs[i].Replace(startKey, String.Empty).Replace("_Ger", String.Empty).Replace("_Eng", String.Empty);
                }
                pairs = pairs.Distinct().OrderByDescending(s => s).ToList();
                var number = pairs.Any() ? int.Parse(pairs.First()) + 1 : 0;

                _newActionTextBox.Text = String.Concat(startKey, number.ToString());
            }
            else
            {
                _newActionTextBox.Text = String.Empty;
            }
        }

        /// <summary>
        /// Adds to data.
        /// </summary>
        /// <param name="newKeyGer">The new key ger.</param>
        /// <param name="newKeyEng">The new key eng.</param>
        private void AddToData(string newKeyGer, string newKeyEng)
        {
            if (!_readData.ContainsKey(newKeyEng))
            {
                _readData.Add(newKeyEng, String.Empty);
            }
            if (!_readData.ContainsKey(newKeyGer))
            {
                _readData.Add(newKeyGer, String.Empty);
            }

            GroupData();
        }

        /// <summary>
        /// Adds to data.
        /// </summary>
        /// <param name="newKeysGer">The new keys ger.</param>
        /// <param name="newKeysEng">The new keys eng.</param>
        private void AddToData(List<string> newKeysGer, List<string> newKeysEng)
        {
            newKeysGer.ForEach(itm => _readData.Add(itm, String.Empty));
            newKeysEng.ForEach(itm => _readData.Add(itm, String.Empty));
        }

        /// <summary>
        /// Splits the text up.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        private void SplitTextUp(string text)
        {
            _readData = new Dictionary<string, string>();
            var splitUp = text.Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var splitted in splitUp)
            {
                var keyValuePair = splitted.Split('\t');
                if (keyValuePair.Count() != 2)
                {
                    continue;
                }

                var filteredKey = keyValuePair[0].Trim();
                var filteredValue = keyValuePair[1].Trim();
                _readData.Add(filteredKey, filteredValue);
            }
        }

        /// <summary>
        /// Saves the file.
        /// </summary>
        private void WriteText()
        {
            StreamWriter writer = new StreamWriter(_filePathTextBox.Text);

            foreach (var pair in _readData)
            {
                string value = String.Concat(pair.Key, "\t", pair.Value, "~");
                writer.WriteLine(value);
            }

            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Reads the text.
        /// </summary>
        private void ReadText()
        {
            try
            {
                TextReader readFile = new StreamReader(_filePathTextBox.Text);
                var content = readFile.ReadToEnd();
                readFile.Close();

                SplitTextUp(content);
                GroupData();
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Groups the data.
        /// </summary>
        private void GroupData()
        {
            var actualyEntry = _keyListBox.SelectedItem?.ToString();

            _keyListBox.Items.Clear();
            _groupedData.Clear();

            var dataSource = _readData.ToDictionary(pair => pair.Key, pair => pair.Value);
            if (_applicationScope == Scope.TextResources)
            {
                dataSource = GroupParts(dataSource, "GangMember", true);
                dataSource = GroupParts(dataSource, "Entry_Eng", false);
                dataSource = GroupParts(dataSource, "_Eng", false);
            }
            else
            {
                dataSource = GroupParts(dataSource, "Name_Eng", false);
            }

            // Add all to the list
            foreach (var pair in _groupedData)
            {
                _keyListBox.Items.Add(pair.Key);
            }

            var itemsOrdered = _keyListBox.Items.Cast<string>().OrderBy(itm => itm).Select(itm => itm).ToArray();
            _keyListBox.Items.Clear();
            _keyListBox.Items.AddRange(itemsOrdered);

            if (_keyListBox.Items.Count > 0 && !string.IsNullOrEmpty(actualyEntry))
            {
                _keyListBox.SelectedIndex = 0;
                _keyListBox.SelectedItem = actualyEntry;
            }
        }

        /// <summary>
        /// Groups the parts.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="groupKey">The group key.</param>
        /// <param name="startsWith">if set to <c>true</c> [starts with].</param>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        private Dictionary<string, string> GroupParts(Dictionary<string, string> dataSource, string groupKey, bool startsWith)
        {
            // Group data.
            var entryKeys = startsWith
                ? dataSource.Where(pair => pair.Key.StartsWith(groupKey)).ToDictionary(pr => pr.Key, pr => pr.Value)
                : dataSource.Where(pair => pair.Key.EndsWith(groupKey)).ToDictionary(pr => pr.Key, pr => pr.Value);

            foreach (KeyValuePair<string, string> pair in entryKeys)
            {
                var key = startsWith
                    ? pair.Key.Substring(0, groupKey.Length)
                    : pair.Key.Substring(0, pair.Key.Length - groupKey.Length);
                var entriesForKey = dataSource.Where(pr => pr.Key.StartsWith(key)).Select(pr => pr.Key).ToList();

                if (entriesForKey.Count > 0)
                {
                    dataSource = dataSource.Where(pr => !pr.Key.StartsWith(key)).ToDictionary(pr => pr.Key, pr => pr.Value);
                    _groupedData.Add(key, entriesForKey);
                }
            }

            return dataSource;
        }

        /// <summary>
        /// Updates a Key
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void _updateButton_Click(object sender, EventArgs e)
        {
            _readData.Remove(_currentKey);

            bool isEng = _currentKeyTextBox.Text.Contains("_Eng");

            _readData.Add(_currentKeyTextBox.Text, isEng ? _englishTextRichTextBox.Text : _germanTextRichTextBox.Text);

            WriteText();
            ReadText();
        }

        /// <summary>
        /// Handle File Path
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void _handleTextRessourceRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            _applicationScope = _handleTextRessourceRadioButton.Checked
                ? Scope.TextResources
                : Scope.ItemResources;

            _filePathTextBox.Text = _applicationScope == Scope.TextResources
                ? @"D:\Unity Projects\GangCards\Assets\Resources\TextResources.txt"
                : @"D:\Unity Projects\GangCards\Assets\Resources\ItemResources.txt";

            _addItemPackageButton.Enabled = _handleItemRessourceRadioButton.Checked;
            _addPairButton.Enabled = _handleTextRessourceRadioButton.Checked;
            _addTextPairButton.Enabled = _handleTextRessourceRadioButton.Checked;
            _addToExistingActionButton.Enabled = _handleTextRessourceRadioButton.Checked;
        }

        /// <summary>
        /// Handles the Click event of the _generateKeyButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _generateKeyButton_Click(object sender, EventArgs e)
        {
            var itemName = _germanyKey.Substring(5, _germanyKey.Length - ("Item_").Length - ("Key").Length);
            _germanTextRichTextBox.Text = Math.Abs(itemName.GetHashCode()).ToString();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the _keyListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _keyListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_keyListBox.SelectedItem != null && _groupedData.ContainsKey(_keyListBox.SelectedItem.ToString()))
            {
                _childrenListBox.DataSource = _groupedData[_keyListBox.SelectedItem.ToString()];

                _textPairTextBox.Text = _keyListBox.SelectedItem.ToString();
            }
        }
    }
}
