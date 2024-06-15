

using DeviceManagementSystem.Core.Entities;
using DeviceManagementSystem.Core.Interfaces;

namespace DeviceManagementSystem.Application.Services
{
    public class DeviceService
    {
        private readonly IDeviceRepository _deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public void AddDevice(Device device)
        {
            _deviceRepository.Add(device);
        }

        public void UpdateDevicePrimaryUser(Guid serialNumber, string newPrimaryUser)
        {
            _deviceRepository.UpdateDevicePrimaryUser(serialNumber, newPrimaryUser);
        }

        public List<Device> GetActiveDevices()
        {
            return _deviceRepository.GetActiveDevices();
        }

        public List<Device> GetAllDevices()
        {
            return _deviceRepository.GetAllDevices();
        }

        public void UpdateDevice(Device device)
        {
            _deviceRepository.Update(device);
        }

        public void DeleteDevice(Guid serialNumber)
        {
            var device = _deviceRepository.GetById(serialNumber);
            if (device != null)
            {
                _deviceRepository.Delete(device);
            }
        }

        public Device GetDeviceById(Guid serialNumber)
        {
            return _deviceRepository.GetById(serialNumber);
        }
    }
}
