namespace HD_Dashboard.Modelos
{
    public class ScoreCard
    {
        public string nombre;

        public int objetivoCombinadas;
        public int realCombinadas;
        public float porcentajeCombinadas;

        public int objetivoTractores;
        public int realTractores;
        public float porcentajeTractores;

        public int objetivoImplementos;
        public int realImplementos;
        public float porcentajeImplementos;

        public int objetivoUsadas;
        public int realUsadas;
        public float porcentajeUsadas;

        public ScoreCard(string nombre, int objetivoCombinadas, int realCombinadas, int objetivoTractores, int realTractores, int objetivoImplementos, int realImplementos, int objetivoUsadas, int realUsadas)
        {
            this.nombre = nombre;
            this.objetivoUsadas = objetivoUsadas;
            this.realUsadas = realUsadas;
            this.objetivoTractores = objetivoTractores;
            this.realTractores = realTractores;
            this.objetivoImplementos = objetivoImplementos;
            this.realImplementos = realImplementos;
            this.objetivoCombinadas = objetivoCombinadas;
            this.realCombinadas = realCombinadas;

            this.porcentajeCombinadas = objetivoCombinadas == 0 ? 1 : realCombinadas / objetivoCombinadas;
            this.porcentajeTractores = objetivoTractores == 0 ? 1 : realTractores / objetivoTractores;
            this.porcentajeImplementos = objetivoImplementos == 0 ? 1 : realImplementos / objetivoImplementos;
            this.porcentajeUsadas = objetivoUsadas == 0 ? 1 : realUsadas / objetivoUsadas;

            this.porcentajeCombinadas = porcentajeCombinadas < 1 ? porcentajeCombinadas * 100 : 100;
            this.porcentajeImplementos = porcentajeImplementos < 1 ? porcentajeImplementos *100 : 100;
            this.porcentajeTractores = porcentajeTractores < 1 ? porcentajeTractores * 100 : 100;
            this.porcentajeUsadas = porcentajeUsadas < 1 ? porcentajeUsadas * 100 : 100;

        }
        public ScoreCard()
        {

        }
     



    }
}
