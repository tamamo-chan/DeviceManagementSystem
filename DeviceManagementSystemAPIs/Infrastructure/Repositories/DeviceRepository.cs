

using DeviceManagementSystem.Core.Entities;
using DeviceManagementSystem.Core.Interfaces;
using DeviceManagementSystem.Infrastructure.Data;

namespace DeviceManagementSystem.Infrastructure.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly DeviceContext _context;

        public DeviceRepository(DeviceContext context)
        {
            _context = context;
        }

        public void Add(Device device)
        {
            _context.Devices.Add(device);
            _context.SaveChanges();
        }

        public void UpdateDevicePrimaryUser(Guid serialNumber, string newPrimaryUser)
        {
            var device = _context.Devices.Find(serialNumber);
            if (device != null)
            {
                device.PrimaryUser = newPrimaryUser;
                _context.SaveChanges();
            }
        }

        public List<Device> GetActiveDevices()
        {
            return _context.Devices.Where(d => d.Status == DeviceStatus.Active).ToList();
        }

        public List<Device> GetAllDevices()
        {
            return _context.Devices.ToList();
        }

        public Device GetById(Guid serialNumber)
        {
            Device? device = _context.Devices.Find(serialNumber);
            return device is null ? throw new ArgumentException("Device ID does not exist!") : device;
        }

        public void Update(Device device)
        {
            Device currentDevice = GetById(device.SerialNumber);
            currentDevice.Status = device.Status;
            currentDevice.PrimaryUser = device.PrimaryUser;
            currentDevice.DeviceType = device.DeviceType;
            currentDevice.OperatingSystem = device.OperatingSystem;
            _context.SaveChanges();
        }

        public void Delete(Device device)
        {
            _context.Devices.Remove(device);
            _context.SaveChanges();
        }
    }
}
