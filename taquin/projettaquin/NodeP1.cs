using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projettaquin
{
    //Classe fille pour le problème Numéro 1

    class NodeP1 : GenericNode
    {
        //coordonnée départ
        private int _coordXD { get; set; }
        private int _coordYD { get; set; }

        //coordonnées arrivée
        private int _coordXA { get; set; }
        private int _coordYA { get; set; }

        private char[,] _map {get; set;}
        private string _end;

        //Récupération des coordonnées du point de départ et du point d'arrivée 
        public NodeP1(string name, string end, char[,] map)
            : base(name)
        {

            _end = end;

            _coordXD = int.Parse(name.Split(',')[0]);
            _coordYD = int.Parse(name.Split(',')[1]);

            _coordXA = int.Parse(end.Split(',')[0]);
            _coordYA = int.Parse(end.Split(',')[1]);

            _map = map;

        }


        public override double GetArcCost(GenericNode N2)
        {
            return (1);
        }


        //Permet de savoir si la position du noeud actuel est la position finale
        public override bool EndState()
        {
            return (_coordXD == _coordXA && _coordYD == _coordYA);
        }



        //Permet de créer la liste des successeurs
        public override List<GenericNode> GetListSucc()  
        {
            char[,] tab = _map;

            int posx = _coordXD;
            int posy = _coordYD;


            List<GenericNode> lsucc = new List<GenericNode>();
            
            
            // Successeur à gauche
            if (posx > 1 && _map[_coordYD, (_coordXD - 1)] != 'w')
            {
                // MAJ de la position du noeud en cours
                string name2 = (_coordXD - 1) + "," + _coordYD;
                
                // Ajout à listsucc
                lsucc.Add(new NodeP1(name2, _end, _map));

                
            }

            // Successeur à droite
            if (posx < 18 && _map[_coordYD, (_coordXD + 1)] != 'w')
            {
                // MAJ de la position du noeud en cours
                string name2 = (_coordXD + 1) + "," + _coordYD;

                // Ajout à listsucc
                lsucc.Add(new NodeP1(name2, _end, _map));
            }
            
            
            // Successeur en haut
            if (posy > 1 && _map[(_coordYD - 1), _coordXD] != 'w')
            {
                // MAJ de la position du noeud en cours
                string name2 = _coordXD + "," + (_coordYD - 1);

                // Ajout à listsucc
                lsucc.Add(new NodeP1(name2, _end, _map));
            }


            // Successeur en bas
            if (posy < 18 && _map[(_coordYD + 1), _coordXD] != 'w')
            {
                // MAJ de la position du noeud en cours
                string name2 =  _coordXD + "," + (_coordYD + 1);
                // Ajout à listsucc
                lsucc.Add(new NodeP1(name2, _end, _map));
            }

            return lsucc;
        }


        public override void CalculeHCost()
        {
            SetEstimation(0);
        }


        //Non utilisé
        /*private string GetStringFromTab ( char [,] tab )
        {
            string newname = "";
            for (int j=0;j<=2; j++)
                for (int i=0;i<=2;i++)
                    {
                       newname = newname + tab[i,j]; 
                   }  
            return newname;
        }*/
    }
}
