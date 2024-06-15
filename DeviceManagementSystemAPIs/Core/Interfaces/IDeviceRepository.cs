

using DeviceManagementSystem.Core.Entities;

namespace DeviceManagementSystem.Core.Interfaces
{
    public interface IDeviceRepository
    {
        void Add(Device device);
        void UpdateDevicePrimaryUser(Guid serialNumber, string newPrimaryUser);
        List<Device> GetActiveDevices();
        List<Device> GetAllDevices();
        Device GetById(Guid id);
        void Update(Device device);
        void Delete(Device device);
    }
}
