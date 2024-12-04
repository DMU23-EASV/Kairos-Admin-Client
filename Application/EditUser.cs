using WPF_MVVM_TEMPLATE.Entitys.DTOs;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Application;

public class EditUser
{
    private readonly IUserRepo _userRepo;

    public EditUser(IUserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    /// <summary>
    /// Asynchronously updates the details of an existing user.
    /// </summary>
    /// <param name="userDTO">A data transfer object containing the updated details for the user.</param>
    /// <returns>Returns the updated user object if the update is successful; otherwise, returns null.</returns>
    /// <exception cref="Exception">Throws an exception if there is an issue with the update process.</exception>
    public async Task<FullUserDTO> UpdateUserAsync(FullUserDTO userDTO)
    {
        try
        {
            var updatedUser = await _userRepo.EditUser(userDTO);

            if (updatedUser != null)
            {
                return updatedUser;
            }
            else
            {
                Console.WriteLine($"UpdateUserAsync: Failed to update user. No response received for user ID: {userDTO.Id}, Username: {userDTO.username}.");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"UpdateUserAsync: Error while updating user ID: {userDTO.Id}, Username: {userDTO.username}. Exception: {ex.Message}");
            throw new Exception($"Error updating user ID: {userDTO.Id}, Username: {userDTO.username}. Exception: {ex.Message}", ex);
        }
    }

}