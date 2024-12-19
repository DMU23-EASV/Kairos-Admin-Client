using System.Windows;
using WPF_MVVM_TEMPLATE.DTO;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;
using WPF_MVVM_TEMPLATE.Presentation.Service;

namespace WPF_MVVM_TEMPLATE.Application
{
    public class CreateUser
    {
        private readonly IUserRepo _userRepo;

        public CreateUser(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        /// <summary>
        /// Method for creating a new user
        /// </summary>
        /// <param name="userDTO">The user data transfer object containing user details</param>
        /// <returns>Returns the created user, or null if the creation fails</returns>
        public async Task<CreateUserDTO> CreateUserAsync(CreateUserDTO userDTO)
        {
            try
            {
                var createdUser = await _userRepo.CreateUser(userDTO);

                if (createdUser != null)
                {
                    Console.WriteLine($"CreateUser: User {createdUser.username} created successfully!");
                    MessageBoxService.Instance.ShowMessageInfo($"Medarbejderen {createdUser.username} blev oprettet med succes", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return createdUser;
                }
                else
                {
                    Console.WriteLine("CreateUser: User creation failed. No response received.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CreateUser: An error occurred while creating the user: {ex.Message}");
                return null;
            }
        }
    }
}