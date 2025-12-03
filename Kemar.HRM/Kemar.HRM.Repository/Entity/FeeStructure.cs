using Kemar.HRM.Repository.Entity.BaseEntities;

namespace Kemar.HRM.Repository.Entity
{
    public class FeeStructure : BaseEntity
    {
        public int FeeStructureId { get; set; }

        public string RoomType { get; set; } = string.Empty;

        public decimal MonthlyRent { get; set; }
        public decimal SecurityDeposit { get; set; }
    }
}
