using WPF_MVVM_TEMPLATE.Presentation.ViewModel;

namespace WPF_MVVM_TEMPLATE.Presentation
{
    /// <summary>
    /// Singleton controller class to manage the lifecycle and registration of ViewModels in a WPF MVVM application.
    /// Provides methods for registering, unregistering, and setting the current ViewModel.
    /// </summary>
    public class ViewModelController : IViewModelController
    {
        // Static field to hold the singleton instance of the ViewModelController.
        private static ViewModelController? _instance;

        /// <summary>
        /// Property to access the singleton instance of the ViewModelController.
        /// If the instance is null, it initializes a new instance.
        /// </summary>
        public static ViewModelController Instance
        {
            get
            {
                // Check if the instance is null, and if so, create a new one.
                if (_instance == null)
                {
                    _instance = new ViewModelController();
                }
                return _instance;
            }
        }
        
        // Public reference to the current application instance, fetched during initialization.
        public readonly App App;

        // Dictionary to store registered ViewModels using their type as the key.
        private readonly Dictionary<Type, ViewModelBase> _viewModels;

        /// <summary>
        /// Private constructor to prevent instantiation from outside the class.
        /// Initializes the dictionary and fetches the current application instance.
        /// </summary>
        private ViewModelController()
        {
            _viewModels = new Dictionary<Type, ViewModelBase>();
            App = FetchCurrentApp(); 
        }

        /// <summary>
        /// Fetches the current application instance cast to the custom App type.
        /// </summary>
        /// <returns>The current App instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the current application instance is not of type App.</exception>
        private App FetchCurrentApp()
        {
            // Ensure the current application instance is of the expected type.
            return (App)App.Current;
        }
        
        /// <summary>
        /// Registers a ViewModel in the controller's dictionary.
        /// </summary>
        /// <param name="viewModel">The ViewModel instance to register.</param>
        /// <exception cref="Exception">
        /// Thrown if the ViewModel type is already registered.
        /// </exception>
        public void RegistryViewModel(ViewModelBase viewModel)
        {
            // Check if the ViewModel type is already registered.
            if (_viewModels.ContainsKey(viewModel.GetType()))
            {
                throw new Exception($"ViewModel {viewModel.GetType().Name} is already registered");
            }

            // Add the ViewModel to the dictionary using its type as the key.
            _viewModels.Add(viewModel.GetType(), viewModel);
        }

        /// <summary>
        /// Unregisters a ViewModel from the controller's dictionary.
        /// </summary>
        /// <param name="viewModelType">The type of the ViewModel to unregister.</param>
        /// <exception cref="Exception">
        /// Thrown if the ViewModel type is not found in the dictionary.
        /// </exception>
        public void UnRegistryViewModel(Type viewModelType)
        {
            // Check if the ViewModel type exists in the dictionary.
            if (!_viewModels.ContainsKey(viewModelType))
            {
                throw new Exception($"ViewModel {viewModelType.Name} is not registered");
            }

            // Remove the ViewModel from the dictionary.
            _viewModels.Remove(viewModelType);
        }

        /// <summary>
        /// Sets the current ViewModel in the application context.
        /// If the ViewModel type is not already registered, it creates a new instance and registers it.
        /// </summary>
        /// <typeparam name="T">The type of the ViewModel, which must inherit from ViewModelBase.</typeparam>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the ViewModel type could not be instantiated.
        /// </exception>
        public void SetCurrentViewModel<T>() where T : ViewModelBase
        {
            // Get the type of the ViewModel.
            var viewModelType = typeof(T);

            // Try to get the ViewModel instance from the dictionary.
            if (!_viewModels.TryGetValue(viewModelType, out ViewModelBase? value))
            {
                // Create a new instance of the ViewModel if it does not exist.
                value = Activator.CreateInstance<T>();
                _viewModels.Add(viewModelType, value);
            }

            // Set the current ViewModel in the application context.
            App.CurrentViewModel = value;
        }
        
        /// <summary>
        /// Retrieves all registered ViewModels as a dictionary of type and instance.
        /// </summary>
        /// <returns>A dictionary with ViewModel types as keys and their instances as values.</returns>
        public Dictionary<Type, ViewModelBase> GetAllViewModels()
        {
            // Return a dictionary of all registered ViewModels.
            return _viewModels;
        }
    }
}
