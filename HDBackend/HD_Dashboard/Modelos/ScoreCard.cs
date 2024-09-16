namespace HD_Dashboard.Modelos
{
    public class ScoreCard
    {
        public string nombre;
        public string hoja;

        public double objetivoCombinadas;
        public double realCombinadas;
        public double porcentajeCombinadas;

        public double objetivoTractores;
        public double realTractores;
        public double porcentajeTractores;

        public double objetivoImplementos;
        public double realImplementos;
        public double porcentajeImplementos;

        public double objetivoUsadas;
        public double realUsadas;
        public double porcentajeUsadas;

        public ScoreCard(string nombre,string hoja, double objetivoCombinadas, double realCombinadas, double objetivoTractores, double realTractores, double objetivoImplementos, double realImplementos, double objetivoUsadas, double realUsadas)
        {
            this.nombre = nombre;
            this.hoja = hoja;
            this.objetivoUsadas = objetivoUsadas;
            this.realUsadas = realUsadas;
            this.objetivoTractores = objetivoTractores;
            this.realTractores = realTractores;
            this.objetivoImplementos = objetivoImplementos;
            this.realImplementos = realImplementos;
            this.objetivoCombinadas = objetivoCombinadas;
            this.realCombinadas = realCombinadas;

            this.porcentajeCombinadas = objetivoCombinadas == 0 && realCombinadas == 0 ? 0 : objetivoCombinadas == 0 && realCombinadas > 0 ? 1 : realCombinadas / objetivoCombinadas;
            this.porcentajeTractores = objetivoTractores == 0 && realTractores == 0 ? 0 : objetivoTractores == 0 && realTractores > 0 ? 1 : realTractores / objetivoTractores;
            this.porcentajeImplementos = objetivoImplementos == 0 && realImplementos == 0 ? 0 : objetivoImplementos == 0 && realImplementos > 0 ? 1 : realImplementos / objetivoImplementos;
            this.porcentajeUsadas = objetivoUsadas == 0 && realUsadas == 0 ? 0 : objetivoUsadas == 0 && realUsadas > 0 ? 1 : realUsadas / objetivoUsadas;

            this.porcentajeCombinadas = porcentajeCombinadas < 1 ? porcentajeCombinadas * 100 : 100;
            this.porcentajeImplementos = porcentajeImplementos < 1 ? porcentajeImplementos *100 : 100;
            this.porcentajeTractores = porcentajeTractores < 1 ? porcentajeTractores * 100 : 100;
            this.porcentajeUsadas = porcentajeUsadas < 1 ? porcentajeUsadas * 100 : 100;

            this.porcentajeCombinadas = Math.Round(this.porcentajeCombinadas, 0);
            this.porcentajeImplementos = Math.Round(this.porcentajeImplementos, 0);
            this.porcentajeTractores = Math.Round(this.porcentajeTractores, 0);
            this.porcentajeUsadas = Math.Round(this.porcentajeUsadas, 0);

        }
        public ScoreCard()
        {

        }
     



    }
}
