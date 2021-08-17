### *Smart DI Container*
# Quick Start

Refer to the [Responsive Tasks Demo](https://github.com/marcusts/Com.MarcusTS.ResponsiveTasksDemo) for the simplest possible way to implement a **Smart Di Container**.

The most important concept here is that a good DI container is not **public static**, but rather as ***private*** as it can ***possibly be*** while still providing instantiation to the app.  For instance, you can create one Smart DI Container for your view models and one for your views. Or even one per module.  Do ***not*** lapse into the ***lazy*** habit of global boot-strapping!

The **ResponsiveTasksDemo** uses a **MasterViewPresenter** to determine what view to show in the app's main page content area:

```csharp
private async Task ChangeContentView<InterfaceT, ClassT>(object viewModel)
   where ClassT : View, InterfaceT
   where InterfaceT : class
{
   // The Smart DI Container allows registering and resolving at the same time !!!
   //    This never double-registers.
   var newView = _diContainer.RegisterAndResolveAsInterface<ClassT, InterfaceT>();
   var newViewAsView = newView as View;

   ...
   
   // Fade the old view out and the new one in

   ... 
   
```

This method is called inside the MasterViewPresenter using a ResponsiveTasks listener that waits for changes to the current view model:

```csharp
private async Task HandleSetCurrentModuleTask(IResponsiveTaskParams paramDict)
{
   ...

   // The view model is passed to us by the responsive task
    var newModule = paramDict[0];

   ...
   
   // Present the appropriate view for the view model
   if (newModule is IDashboardViewModel)
   {
      await ChangeContentView<IDashboardView, DashboardView>(newModule);
   }
   else if (newModule is IAccountViewModel)
   {
      await ChangeContentView<IAccountView, AccountView>(newModule);
   }
   else if (newModule is ISettingsViewModel)
   {
      await ChangeContentView<ISettingsView, SettingsView>(newModule);
   }

   _lastModule = newModule;
}
```

The view model is also created and maintained inside a separate **Smart DI Container**.  This one is housed in the AppStateManager, which determines what view model matches a given set of run-time conditions:

```csharp
private async Task ChangeState<InterfaceT, ClassT>(string newState)
   where ClassT : class, InterfaceT
   where InterfaceT : class
{
   // Another painless register and resolve
   var viewModel = DIContainer.RegisterAndResolveAsInterface<ClassT, InterfaceT>();

   if (viewModel is ITitledViewModel viewModelAsTitled)
   {
      viewModelAsTitled.Title = TitleFromPairs(newState);
   }
   
   // Sets up the various related controls (including the toolbar) for the view model
   await SetSelectionKey(newState);

   // Sets the new view and thena assigns the binding contest to the new view model.
   await SetCurrentModuleTask.RunAllTasksUsingDefaults(viewModel);
}
```


