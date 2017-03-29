namespace Material.Domain.Bluetooth
{
    public class BluetoothSpecification
    {
        public string SpecificationName { get; }
        public string SpecificationType { get; }
        public int AssignedNumber { get; }

        public BluetoothSpecification(
            string specificationName,
            string specificationType,
            int assignedNumber)
        {
            SpecificationName = specificationName;
            SpecificationType = specificationType;
            AssignedNumber = assignedNumber;
        }
    }
}
