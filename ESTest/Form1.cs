using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ESTest
{
    

    public partial class Form1 : Form
    {
        int count = 0;
        List<Question> Test = new List<Question>(); 
        public Form1()
        {
            InitializeComponent();
        }
        bool is_editing = false;
        
        bool save = false;
        string saveFilename;
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //Создание новых вопрсов
        private void сВыборомОтветаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            is_editing = false;
            SA.Visible = false;
            CMA.Visible = false;
            COA.Visible = true;
            textBox1.Text = "";
            richTextBox1.Text = "";
            numericUpDown1.Value = 1;

        }
        private void сНесколькимиВариантамиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            is_editing = false;
            COA.Visible = false;
            SA.Visible = false;
            CMA.Visible = true;
            textBox2.Text = "";
            richTextBox2.Text = "";
            richTextBox3.Text = "";
        }
        private void сКраткимОтветомToolStripMenuItem_Click(object sender, EventArgs e)
        {
            is_editing = false;
            COA.Visible = false;
            CMA.Visible = false;
            SA.Visible = true;
            textBox3.Text = "";
            textBox4.Text = "";

        }
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //Сохранение вопросов
        private void button3_Click(object sender, EventArgs e)
        {
            if (is_editing == false)
            {
                
                string problem;
                problem = textBox1.Text;
                string[] answer;
                answer = richTextBox1.Text.Split('\n');
                int rAnswer;
                rAnswer = Convert.ToInt32(numericUpDown1.Value);
                Test.Add(new QuestionOneChoice(problem, answer, rAnswer));
                if (treeView1.Nodes.Count == 0)
                    treeView1.Nodes.Add(new TreeNode("Новый тест"));
                treeView1.Nodes[0].Nodes.Add(new TreeNode(problem));
            }
            else
            {               
                Test[count].Problem = textBox1.Text;                
                Test[count].Answers = richTextBox1.Text.Split('\n');
                (Test[count] as QuestionOneChoice).RightAnswer = Convert.ToInt32(numericUpDown1.Value);
                
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            if (richTextBox3.Text == "")
                MessageBox.Show("Не введены правильные ответы", "Ошибка", buttons);
            else
            {
                if (is_editing == false)
                {
                    
                    string problem;
                    problem = textBox2.Text;
                    string[] answer;
                    answer = richTextBox2.Text.Split('\n');
                    int[] rAnswer;

                    rAnswer = richTextBox3.Text.Split(';').Select(x => Convert.ToInt32(x)).ToArray();
                    Test.Add(new QuestionMultyChoice(problem, answer, rAnswer));
                    if (treeView1.Nodes.Count == 0)
                        treeView1.Nodes.Add(new TreeNode("Новый тест"));
                    treeView1.Nodes[0].Nodes.Add(new TreeNode(problem));
                }
                else
                {
                    Test[count].Problem = textBox2.Text;
                    Test[count].Answers = richTextBox2.Text.Split('\n');
                    (Test[count] as QuestionMultyChoice).RightAnswer = richTextBox3.Text.Split(';').Select(x => Convert.ToInt32(x)).ToArray();
                }
            }

        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (is_editing == false)
            {
                
                string problem;
                problem = textBox3.Text;
                string rAnswer;
                rAnswer = textBox4.Text.ToLower();
                Test.Add(new QuestionShort(problem, rAnswer));
                if (treeView1.Nodes.Count == 0)
                    treeView1.Nodes.Add(new TreeNode("Новый тест"));
                treeView1.Nodes[0].Nodes.Add(new TreeNode(problem));
            }
            else
            {
                Test[count].Problem = textBox3.Text;
                (Test[count] as QuestionShort).RightAnswer = textBox4.Text.ToLower();
                
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        //Сохранение теста

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (save == false)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                //saveFileDialog1.ShowDialog();
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                string filename = saveFileDialog1.FileName;
                XML_Test_Writer.Write(Test, filename);
                saveFilename = filename;
                save = true;
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);
                filename = filename.Substring(0, filename.LastIndexOf("."));
                treeView1.Nodes[0].Text = filename;
            }
            else
            {
                string filename = saveFilename;
                XML_Test_Writer.Write(Test, filename);
                save = true;
            }

        }

        //Сохранить как
        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            //saveFileDialog1.ShowDialog();
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = saveFileDialog1.FileName;
            XML_Test_Writer.Write(Test, filename);
            saveFilename = filename;
            save = true;
            filename = filename.Substring(filename.LastIndexOf("\\") + 1);
            filename = filename.Substring(0, filename.LastIndexOf("."));
            treeView1.Nodes[0].Text = filename;
        }

        //Создание нового теста
        private void сОздатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            string caption = "Создать новый тест?";
            string message = "Вы хотите создать новый тест?" + "\n" + "(Несохраненные данные будут утеряны)";
            result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(new TreeNode("Новый тест"));
            }

        }
        //редактирование имени узла
        private void treeView1_DoubleClick(object sender, System.EventArgs e)
        {
            treeView1.SelectedNode.BeginEdit();
        }
        //Редактирование
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            is_editing = true;
            count = e.Node.Index;
            int type = Test[count].GetQuestionType();
            switch (type)
            {
                case 0:
                    SA.Visible = false;
                    CMA.Visible = false;
                    COA.Visible = true;
                    textBox1.Text = "";
                    richTextBox1.Text = "";
                    numericUpDown1.Value = 1;

                    textBox1.Text = this.Test[count].GetProblem();
                    richTextBox1.Lines = this.Test[count].GetAnswers();
                    numericUpDown1.Value = (this.Test[count] as QuestionOneChoice).GetRightAnswer();

                    break;

                case 1:
                    SA.Visible = false;
                    CMA.Visible = true;
                    COA.Visible = false;
                    richTextBox3.Text = "";
                    textBox2.Text = this.Test[count].GetProblem();
                    richTextBox2.Lines = this.Test[count].GetAnswers();                    
                    int[] RightAnswers = (this.Test[count] as QuestionMultyChoice).GetRightAnswers();
                    foreach (int ra in RightAnswers)
                    {
                        richTextBox3.AppendText(ra.ToString());
                        richTextBox3.Text+=";";
                    }
                    richTextBox3.Text = richTextBox3.Text.TrimEnd(';');
                    break;
                    
                case 2:

                    SA.Visible = true;
                    CMA.Visible = false;
                    COA.Visible = false;
                    textBox3.Text = this.Test[count].GetProblem();
                    textBox4.Text = (this.Test[count] as QuestionShort).GetRightAnswer();
                    
                    break;
            }

        }
        //удаление
        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            count = treeView1.SelectedNode.Index;
            Test.RemoveAt(count)    ;
            treeView1.Nodes[0].Nodes.Remove(treeView1.SelectedNode);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Вы уверены, что хотите выйти?";
            string caption = "Выйти?";
            MessageBoxButtons button = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MessageBox.Show(message, caption, button);
            if (result == System.Windows.Forms.DialogResult.Yes)
                this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void COA_Paint(object sender, PaintEventArgs e)
        {

        }
        // открытие теста
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
           Test =  XML_Test_Reader.Read(filename);
            filename = filename.Substring(filename.LastIndexOf("\\") + 1);
            filename = filename.Substring(0, filename.LastIndexOf("."));
            treeView1.Nodes.Add(new TreeNode(filename));
            foreach (Question Questions in Test)
            {
                string problem = Test[count].Problem;
                treeView1.Nodes[0].Nodes.Add(new TreeNode(problem));
            }

           
        }
    }

    public static class XML_Test_Reader
    {
        public static List<Question> Read(string filename)
        {
            List<Question> Test = new List<Question>();
            using (XmlReader reader = XmlReader.Create(filename))
            {
                while (reader.Name != "Question")
                    reader.Read();
                while (reader.Name == "Question") // Посмотреть, что там происходит
                {
                    int type = Convert.ToInt32(reader.GetAttribute("type"));
                    reader.Read();
                    reader.Read();
                    string problem = reader.Value;
                    reader.Read();
                    reader.Read(); // Нужно ещё одно добавить?
                    List<string> answers = new List<string>();
                    if (type != 2)
                    {
                        reader.Read();
                        while (reader.Name == "Answer")
                        {
                            reader.Read();
                            answers.Add(reader.Value);
                            reader.Read();
                            reader.Read();
                        }
                        reader.Read();
                    }
                    else
                        reader.Read();
                    reader.Read();
                    switch (type)
                    {
                        case 0:
                        case 1:
                            List<int> rightAnswers = new List<int>();
                            while (reader.Name == "RightAnswer")
                            {
                                reader.Read();
                                rightAnswers.Add(Convert.ToInt32(reader.Value));
                                reader.Read();
                                reader.Read();
                            }
                            if (type == 0)
                                Test.Add(new QuestionOneChoice(problem, answers.ToArray(), rightAnswers[0]));
                            else
                                Test.Add(new QuestionMultyChoice(problem, answers.ToArray(), rightAnswers.ToArray()));
                            reader.Read();
                            reader.Read();
                            break;
                        case 2:
                            reader.Read();
                            Test.Add(new QuestionShort(problem, reader.Value));
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            break;
                    }

                }

            }
            return Test;
        }
    }

    public static class XML_Test_Writer
    {
       

        public static void Write(List<Question> list, string filename)
        {
            using (XmlWriter writer = XmlWriter.Create(filename))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Test");
                foreach (Question question in list)
                {
                    int type = question.GetQuestionType();
                    writer.WriteStartElement("Question");
                    writer.WriteAttributeString("type", type.ToString());
                    writer.WriteElementString("Problem", question.GetProblem());
                    string[] answers = question.GetAnswers();
                    writer.WriteStartElement("Answers");
                    foreach (string answer in answers)
                    {
                        writer.WriteElementString("Answer", answer);
                    }
                    writer.WriteEndElement(); //Answers
                    writer.WriteStartElement("RightAnswers");
                    switch (type)
                    {
                        case 0:
                            int RightAnswer = (question as QuestionOneChoice).GetRightAnswer();
                            writer.WriteElementString("RightAnswer", RightAnswer.ToString());
                            break;
                        case 1:
                            int[] RightAnswers = (question as QuestionMultyChoice).GetRightAnswers();
                            foreach (int ra in RightAnswers)
                                writer.WriteElementString("RightAnswer", ra.ToString());
                            break;
                        case 2:
                            writer.WriteElementString("RightAnswer", (question as QuestionShort).GetRightAnswer());
                            break;
                    }
                    writer.WriteEndElement(); //RightAnswers
                    writer.WriteEndElement(); //Question
                }


                writer.WriteEndElement(); //Test
                writer.WriteEndDocument();
            }
        }
    }

    public abstract class Question
    {
        public string Problem;
        public string[] Answers;

        public abstract string GetProblem();
        public abstract int GetQuestionType();
        public abstract string[] GetAnswers();
    }
    public class QuestionOneChoice : Question
    {
        public int RightAnswer;

        public QuestionOneChoice(string Problem, string[] Answers, int RightAnswer)
        {
            this.Problem = Problem;
            this.Answers = Answers;
            this.RightAnswer = RightAnswer;
        }

        public override string GetProblem()
        {
            return this.Problem;
        }
        public override int GetQuestionType()
        {
            return 0;
        }
        public override string[] GetAnswers()
        {
            return Answers;
        }
        internal int GetRightAnswer()
        {
            return RightAnswer;
        }
    }

    public class QuestionMultyChoice : Question
    {
        public int[] RightAnswer;
        public QuestionMultyChoice(string Problem, string[] Answers, int[] RightAnswer)
        {
            this.Problem = Problem;
            this.Answers = Answers;
            this.RightAnswer = RightAnswer;
        }
        public override string GetProblem()
        {
            return this.Problem;
        }

        public override int GetQuestionType()
        {
            return 1;
        }

        public override string[] GetAnswers()
        {
            return Answers;
        }
        internal int[] GetRightAnswers()
        {
            return RightAnswer;
        }
    }
    public class QuestionShort : Question
    {
        public string RightAnswer;
        public QuestionShort(string Problem, string RightAnswer)
        {
            this.Problem = Problem;
            this.RightAnswer = RightAnswer;
        }
        public override string GetProblem()
        {
            return this.Problem;
        }

        public override int GetQuestionType()
        {
            return 2;
        }
        public override string[] GetAnswers()
        {
            return new string[0];
        }
        internal string GetRightAnswer()
        {
            return RightAnswer;
        }
    }
}
