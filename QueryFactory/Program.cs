using System;

namespace QueryFactory
{
    class Program
    {

        public class RotaInspetor
        {
            public int IdRotaInspetor { get; set; }
            public int IdInspetor { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public DateTime Data { get; set; }
        }

        public class Notificacao
        {
            public int IdNotificacao { get; set; }
            public int IdInspecao { get; set; }
            public int IdUsuarioTipoNotificacao { get; set; }
            public string Mensagem { get; set; }
            public DateTime DataEnvio { get; set; }
            public bool FoiEnviado { get; set; }
            public string Destinatario { get; set; }
        }

        public class Estrutura
        {
            public int IdEstrutura { get; set; }
            public int IdEmpreendimento { get; set; }
            public int? IdBarragem { get; set; }
            public int? IdEstruturaPai { get; set; }
            public int IdTipoEstrutura { get; set; }
            public string Nome { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public int MapaZoom { get; set; }
            public string CaminhoImagem { get; set; }
            public string CaminhoCamada { get; set; }
            public DateTime? DataExclusao { get; set; }
            public int? IdResponsavel { get; set; }
        }

        public class Inspecao
        {
            public int IdInspecao { get; set; }
            public int IdEstrutura { get; set; }
            public int IdTipoInspecao { get; set; }
            public int IdResponsavel { get; set; }
            public DateTime DataInicio { get; set; }
            public DateTime? DataTermino { get; set; }
            public DateTime DataCriacao { get; set; }
            public int IdStatusInspecao { get; set; }
            public bool EhWeb { get; set; }
            public DateTime? DataProx { get; set; }
            public string Identificador { get; set; }
            public DateTime? DataExclusao { get; set; }
        }

        static void Main(string[] args)
        {
            IQueryFactory query1 = new QueryFactory<RotaInspetor>();
            IQueryFactory query2 = new QueryFactory<Notificacao>();
            var a = new QueryFactory<RotaInspetor>();
            //a.Select().teste()
            //query1.Select().
            //query2.Select<RotaInspetor>().; 


            var asda = QueryFactory
                .Select(typeof(Inspecao))
                .Join(JoinType.InnerJoin, typeof(Estrutura), nameof(Estrutura.IdEstrutura), true)
                .On(nameof(Inspecao), nameof(Inspecao.IdEstrutura))
                .Create();

            int x = 0;
        }
    }
}
