namespace ServicioPAI.Entidades
{
    public class PersonaEntity
    {
        public long per_Consecutivo { get; set; }
        public string per_TipoId { get; set; }
        public string per_Id { get; set; }
        public string per_CertNacVivo { get; set; }
        public string per_CertDefuncion { get; set; }
        public string per_TipoIdM { get; set; }
        public string per_IdM { get; set; }
        public short per_NumeroHijoM { get; set; }
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }
        public string primerNombre { get; set; }
        public string segundoNombre { get; set; }
        public string primerApellidoM { get; set; }
        public string segundoApellidoM { get; set; }
        public string primerNombreM { get; set; }
        public string segundoNombreM { get; set; }
        public DateTime perFechaNac { get; set; }
        public string per_parInstitucion { get; set; }
        public DateTime per_FechaAlm { get; set; }
        public string per_Func { get; set; }
        public string per_Institucion { get; set; }
        public int per_Estado { get; set; }
        public int cni_id { get; set; }
        public short etn_idEtnia { get; set; }
        public string gru_IdGrupo { get; set; }
        public string per_Genero { get; set; }
        public string perGrupoSanguineo { get; set; }
        public string perRh { get; set; }
        public string pes_Peso { get; set; }
        public int per_causaNoVacuna { get; set; }
        public long per_ConsecutivoM { get; set; }
        public string hisDocumentJson { get; set; }          
    }
}
