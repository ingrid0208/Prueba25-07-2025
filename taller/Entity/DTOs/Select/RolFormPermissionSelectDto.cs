namespace Entity.DTOs.Select
{
    public class RolFormPermissionSelectDto
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public int FormId { get; set; }
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string RolName { get; set; }
        public string FormName { get; set; }
    }
}
