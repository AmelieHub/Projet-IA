using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace projettaquin
{
    public partial class Form1 : Form
    {
        private char[,] map;
        private List<GenericNode> Lres;
        private int nbAffichee;

        public Form1()
        {
            InitializeComponent();
            map = new char[,]
            {
                {'w','w','w','w','w','w','w','w','w','w','w','w','w','w','w','w','w','w','w','w'},
                {'w','a','a','a','a','a','a','a','a','-','d','d','d','d','w','e','e','e','e','w'},
                {'w','a','a','a','a','a','a','a','a','-','d','d','d','d','w','e','e','e','e','w'},
                {'w','a','a','a','a','a','a','a','a','w','d','d','d','d','w','e','e','e','e','w'},
                {'w','a','a','a','a','a','a','a','a','w','d','d','d','d','w','e','e','e','e','w'},
                {'w','w','w','w','w','w','w','-','-','w','d','d','d','d','w','e','e','e','e','w'},
                {'w','b','b','b','b','b','b','b','b','w','d','d','d','d','w','e','e','e','e','w'},
                {'w','b','b','b','b','b','b','b','b','w','-','-','w','w','w','w','w','-','-','w'},
                {'w','b','b','b','b','b','b','b','b','w','f','f','f','f','f','f','f','f','f','w'},
                {'w','b','b','b','b','b','b','b','b','w','f','f','f','f','f','f','f','f','f','w'},
                {'w','b','b','b','b','b','b','b','b','w','f','f','f','f','f','f','f','f','f','w'},
                {'w','w','w','w','w','w','w','-','-','w','-','-','f','f','f','f','f','f','f','w'},
                {'w','c','c','c','c','c','c','c','c','c','c','c','w','w','w','w','w','-','-','w'},
                {'w','c','c','c','c','c','c','c','c','c','c','c','w','g','g','g','g','g','g','w'},
                {'w','c','c','c','c','c','c','c','c','c','c','c','w','g','g','g','g','g','g','w'},
                {'w','c','c','c','c','c','c','c','c','c','c','c','w','g','g','g','g','g','g','w'},
                {'w','c','c','c','c','c','c','c','c','c','c','c','w','g','g','g','g','g','g','w'},
                {'w','c','c','c','c','c','c','c','c','c','c','c','w','g','g','g','g','g','g','w'},
                {'w','c','c','c','c','c','c','c','c','c','c','c','w','g','g','g','g','g','X','w'},
                {'w','w','w','w','w','w','w','w','w','w','w','w','w','w','w','w','w','w','w','w'}
            };

            resetCouleurMap();

            nbAffichee = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;

            // reset
            Lres = null;
            nbAffichee = 0;
            resetCouleurMap();
            listBox1.Items.Clear();

            // sécurité
            string[] sec = textBox1.Text.Split(',');
            int x, y;
            if (sec.Length != 2 || !int.TryParse(sec[0], out x) && !int.TryParse(sec[1], out y))
            {
                textBox1.Text = "1,1";
            }

            sec = textBox2.Text.Split(',');
            if (sec.Length != 2 || !int.TryParse(sec[0], out x) && !int.TryParse(sec[1], out y))
            {
                textBox2.Text = "1,1";
            }

            sec = textBox3.Text.Split(',');
            if (sec.Length != 2 || !int.TryParse(sec[0], out x) && !int.TryParse(sec[1], out y))
            {
                textBox3.Text = "1,1";
            }

            // création du graph
            Graph g = new Graph();

            // path finding 1
            NodeP1 N0 = new NodeP1(textBox1.Text, textBox2.Text, map);
            Lres = g.RechercheSolutionAEtoile(N0);

            // path finding 2
            NodeP1 N1 = new NodeP1(textBox2.Text, textBox3.Text, map);
            Lres.AddRange(g.RechercheSolutionAEtoile(N1));

            if (Lres.Count == 0)
            {
                labelsolution.Text = "Pas de solution";
            }
            else
            {
                labelsolution.Text = "Une solution a été trouvée";
                foreach (GenericNode N in Lres)
                {
                    listBox1.Items.Add(N);
                }
                labelcountopen.Text = "Nb noeuds des ouverts : " + g.CountInOpenList().ToString();
                labelcountclosed.Text = "Nb noeuds des fermés : " + g.CountInClosedList().ToString();
                g.GetSearchTree(treeView1);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (nbAffichee >= Lres.Count())
            {
                timer1.Enabled = false;
                nbAffichee = 0;
            }
            else
            {
                GenericNode n = null;
                if(Lres[nbAffichee] is NodeP1)
                    n = Lres[nbAffichee] as NodeP1;
                if(Lres[nbAffichee] is NodeP2)
                    n = Lres[nbAffichee] as NodeP2;
                //if(Lres[nbAffichee] is NodeP3)
                //    n = Lres[nbAffichee] as NodeP3;
                int row = int.Parse(n.GetNom().Split(',')[0]);
                int column = int.Parse(n.GetNom().Split(',')[1]);
                if (tableLayoutPanel1.GetControlFromPosition(column, row) is PictureBox)
                {
                    PictureBox p = tableLayoutPanel1.GetControlFromPosition(row, column) as PictureBox;
                    p.BackColor = Color.Red;
                }
                nbAffichee++;
            }
        }

        private void resetCouleurMap()
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (tableLayoutPanel1.GetControlFromPosition(j, i) is PictureBox)
                    {
                        PictureBox p = tableLayoutPanel1.GetControlFromPosition(j, i) as PictureBox;
                        if (map[i, j] == 'w')
                            p.BackColor = Color.Black;
                        else if (map[i, j] == '-')
                            p.BackColor = Color.Silver;
                        else
                            p.BackColor = Color.White;
                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;

            // reset
            Lres = null;
            nbAffichee = 0;
            resetCouleurMap();
            listBox1.Items.Clear();

            // sécurité
            string[] sec = textBox1.Text.Split(',');
            int x, y;
            if (sec.Length != 2 || !int.TryParse(sec[0], out x) && !int.TryParse(sec[1], out y))
            {
                textBox1.Text = "1,1";
            }

            sec = textBox2.Text.Split(',');
            if (sec.Length != 2 || !int.TryParse(sec[0], out x) && !int.TryParse(sec[1], out y))
            {
                textBox2.Text = "1,1";
            }

            sec = textBox3.Text.Split(',');
            if (sec.Length != 2 || !int.TryParse(sec[0], out x) && !int.TryParse(sec[1], out y))
            {
                textBox3.Text = "1,1";
            }

            // création du graph
            Graph g = new Graph();

            // path finding 1
            NodeP2 N0 = new NodeP2(textBox1.Text, textBox2.Text, map);
            Lres = g.RechercheSolutionAEtoile(N0);

            // path finding 2
            NodeP2 N1 = new NodeP2(textBox2.Text, textBox3.Text, map);
            Lres.AddRange(g.RechercheSolutionAEtoile(N1));

            if (Lres.Count == 0)
            {
                labelsolution.Text = "Pas de solution";
            }
            else
            {
                labelsolution.Text = "Une solution a été trouvée";
                foreach (GenericNode N in Lres)
                {
                    listBox1.Items.Add(N);
                }
                labelcountopen.Text = "Nb noeuds des ouverts : " + g.CountInOpenList().ToString();
                labelcountclosed.Text = "Nb noeuds des fermés : " + g.CountInClosedList().ToString();
                g.GetSearchTree(treeView1);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            timer1.Enabled = true;

            // reset
            Lres = null;
            nbAffichee = 0;
            resetCouleurMap();
            listBox1.Items.Clear();

            // sécurité
            string[] sec = textBox1.Text.Split(',');
            int x, y;
            if (sec.Length != 2 || !int.TryParse(sec[0], out x) && !int.TryParse(sec[1], out y))
            {
                textBox1.Text = "1,1";
            }

            sec = textBox2.Text.Split(',');
            if (sec.Length != 2 || !int.TryParse(sec[0], out x) && !int.TryParse(sec[1], out y))
            {
                textBox2.Text = "1,1";
            }

            sec = textBox3.Text.Split(',');
            if (sec.Length != 2 || !int.TryParse(sec[0], out x) && !int.TryParse(sec[1], out y))
            {
                textBox3.Text = "1,1";
            }

            // création du graph
            Graph g = new Graph();

            // path finding 1
            NodeP1 N0 = new NodeP1(textBox1.Text, textBox2.Text, map);
            Lres = g.RechercheSolutionAEtoile(N0);

            // path finding 2
            NodeP1 N1 = new NodeP1(textBox2.Text, textBox3.Text, map);
            Lres.AddRange(g.RechercheSolutionAEtoile(N1));

            if (Lres.Count == 0)
            {
                labelsolution.Text = "Pas de solution";
            }
            else
            {
                labelsolution.Text = "Une solution a été trouvée";
                foreach (GenericNode N in Lres)
                {
                    listBox1.Items.Add(N);
                }
                labelcountopen.Text = "Nb noeuds des ouverts : " + g.CountInOpenList().ToString();
                labelcountclosed.Text = "Nb noeuds des fermés : " + g.CountInClosedList().ToString();
                g.GetSearchTree(treeView1);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
