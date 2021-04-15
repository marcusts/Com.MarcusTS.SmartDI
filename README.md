## Finally, a DI Container Without The Fake "IOC" Swagger

#### _It's just dependency injection - that's all, folks_

Modern programmers require some means of creating new classes out of other classes that are passed as parameters, and to do so in an elegant, seamless manner. Those new instances often require _caching_, especially if they are services or view models. The got-to solution for this challenge has been the mis-named and grossly mis-described _"IOC Container"_. In [another article](https://marcusts.com/2018/04/09/the-ioc-container-anti-pattern/), I explained why that is a bad idea. If we can't even name something accurately, it is time to reconsider it entirely. So I have done that here with the tool that all of us actually need: a DI _("Dependency Injection")_ Container.

## What's So Smart About It?

#### 1\. It provides true _**lifecycle management**_

IOC containers always store an instance when you create it. That is extremely _**wasteful**_. It is also _**unsafe**_. The **Smart DI Container** provides only three types of registrations:

*   **Any Access Level / Do Not Store:** Use these to create instances at any time and without caching. The local variable you retrieve on Resolve() is the only stored reference.![](https://marcusts.com/wp-content/uploads/2018/12/AnyAccessLevel2.png)

*   **Shared Between Instances:** When you Resolve() using this access level, you must to pass in a "parent" object that gets indexed to that new class instance. You can also link the same instance to any number of other consumers/parents by calling Resolve() again. Once all of the parents die, the cached instance is also removed. _Note: This requires you to raise a Xamarin.Forms event to notify the container about the death of a shared instance parent. We do this for you if you also use our [Lifecycle Aware Library](https://marcusts.com/2018/05/01/taking-control-of-variable-lifecycle/)._![](https://marcusts.com/wp-content/uploads/2018/12/SharedAccessLevel.png)

*   **Global Singleton:** The container creates and caches a permanent instance of any type registered with this access level. The cached reference dies when the container itself falls out of scope.![](https://marcusts.com/wp-content/uploads/2018/12/GlobalSingleton.png)

In spite of the ginormous size of other containers on the market, none of them can pass this test. The container must provide a **_physical mechanism_** to make this functionality possible. We have one!

#### 2\. It is not global or static

You can declare an instance of the **Smart DI Container** wherever you please. This supports "nested" scenarios, where containers live within narrowly defined class inheritance trees. **_Remember:_** all "global" variables stored/cached will only live as long as the container does.

#### 3\. It is well-behaved

The **Smart DI Container** protects against recursive calls, or any other violation of the rules-based registrations you make. For instance, if you register two competing interfaces for the same base type:

``` C#
_container = new SmartDIContainer();
_container.RegisterTypeAsInterface<FirstSimpleClass>(typeof(IAmSimple));
_container.RegisterTypeAsInterface<SecondSimpleClass>(typeof(IAmSimple));
```

... and then resolve **IAmSimple**, you have created a _**conflict**_. The container cannot know which one to return. You can set a Boolean property to throw an error in this case. Or you can provide a _conflict resolver_:

``` C#
var simple = _container.Resolve<IAmSimple>(StorageRules.AnyAccessLevel, null, ForbidSpecificClass<FirstSimpleClass>);

private static IConflictResolution ForbidSpecificClass<T>(IDictionary<Type, ITimeStampedCreatorAndStorageRules> registrations)
{
   // Find any registration where the key (the main class that was registered and that is being constructed) is *not* the forbidden one
   var legalValues = registrations.Where(r => r.Key != typeof(T)).ToArray();

   if (legalValues.IsEmpty())
   {
      return null;
   }

   return new ConflictResolution
                {
                   MasterType                = legalValues.First().Key,
                   TypeToCastWithStorageRule = legalValues.First().Value.CreatorsAndStorageRules.First()
                };
      }
```

#### 4\. It is **_tiny_**

**Smart DI Container** occupies almost no space at all, and rarely touches memory, since it does not store anything unnecessarily.

#### 5\. It's basic, honest open source C#, and easy to read.

We actually added comments! _(And we were not struck by lightning)_

#### 6\. It is tested and proven

See the [unit tests](https://github.com/marcusts/SafeDiContainer).

## Quick Start

#### Create DI Containers Wherever You Need Them; Do Not Worry About Centralization

DI Containers both register and provide access to variables. To stay within the **C# SOLID** Guidance, your app should be as _**private as possible**_. So the last thing you need are global containers. Services are a notable exception. They should be available generally throughout the app. So **app.xaml.cs** might look like this:

``` C#
   public partial class App : Application, IManagePageChanges, IReportAppLifecycle
   {
      public static readonly ISmartDIContainer GlobalServiceContainer = new SmartDIContainer();

      public App()
      {
         InitializeComponent();

         // Required by IOS
         MainPage = new ContentPage();

         GlobalServiceContainer.RegisterTypeAsInterface<GlobalServiceOne>(typeof(IGlobalServiceOne), StorageRules.GlobalSingleton);
         GlobalServiceContainer.RegisterTypeAsInterface<GlobalServiceTwo>(typeof(IGlobalServiceTwo), StorageRules.GlobalSingleton);
         GlobalServiceContainer.RegisterTypeAsInterface<GlobalServiceThree>(typeof(IGlobalServiceThree), StorageRules.GlobalSingleton);
         GlobalServiceContainer.RegisterTypeAsInterface<ViewModelFactory>(typeof(IViewModelFactory));

         // Start up the navigation system
         StateMachine.ResetCurrentPageMode();
      }
   }
```

#### Create a View Model Factory

``` C#
   public class ViewModelFactory : IViewModelFactory
   {
      #region Private Fields

      private readonly SmartDIContainerWithLifecycle _viewModelContainer = new SmartDIContainerWithLifecycle();

      #endregion Private Fields

      #region Public Constructors

      public ViewModelFactory(IGlobalServiceOne   service1,
                              IGlobalServiceTwo   service2,
                              IGlobalServiceThree service3)
      {
         // Register the services as singletons
         _viewModelContainer.RegisterTypeAsInterface<GlobalServiceOne>(typeof(IGlobalServiceOne), StorageRules.GlobalSingleton);
         _viewModelContainer.RegisterTypeAsInterface<GlobalServiceTwo>(typeof(IGlobalServiceTwo), StorageRules.GlobalSingleton);
         _viewModelContainer.RegisterTypeAsInterface<GlobalServiceThree>(typeof(IGlobalServiceThree), StorageRules.GlobalSingleton);

         // Register other known types using various access levels
         _viewModelContainer.RegisterTypeAsInterface<ViewModel_Private>(typeof(IViewModel_Private), StorageRules.DoNotStore);
         _viewModelContainer.RegisterTypeAsInterface<ViewModel_ToBeShared>(typeof(IViewModel_ToBeShared), StorageRules.SharedDependencyBetweenInstances);
         _viewModelContainer.RegisterTypeAsInterface<ViewModel_Global>(typeof(IViewModel_Global), StorageRules.GlobalSingleton);
      }

      #endregion Public Constructors

      #region Public Methods

      /// <summary>
      /// This case requires an object "parent".
      /// </summary>
      public ICustomViewModelBase CreateSharedViewModel<T>(object obj) where T : class, ICustomViewModelBase
      {
         return _viewModelContainer.Resolve<T>(boundInstance: obj);
      }

      /// <summary>
      /// This method will return the view model based on its registration rules.
      /// It is safer than registering as "All Access" and then resolving using more narrow guidance.
      /// It also encapsulates the private (static) view model container, insulating it from the rest of the app.
      /// </summary>
      public ICustomViewModelBase CreateViewModel<T>() where T : class, ICustomViewModelBase
      {
         return _viewModelContainer.Resolve<T>();
      }
```

We add **ViewModel.Utils** to give us access to the view model factory:

``` C#   public static class ViewModelUtils
   {
      /// <summary>
      /// Get the view model factory out of the main container; the services are provided at the same time.
      /// </summary>
      public static readonly IViewModelFactory ViewModelBuilder =
         App.GlobalServiceContainer.Resolve<IViewModelFactory>();
   }
```

The **View Model Factory** requires three services at its constructor. By resolving the factory from the global container _(over at **app.xaml.cs**)_, those services get injected automatically, so are now available for consumption by us here.

#### Generate a View Model Base to do the Heavy Lifting for the View Models

In this simple app, the view models don't provide much differentiation, so everything goes into the base class. Note that the "next" button command is shared; it just asks us to navigate.

``` C#   [AddINotifyPropertyChangedInterface]
public class CustomViewModelBase : ViewModelWithLifecycle, ICustomViewModelBase
{
   public ICommand ButtonCommand => new Command(StateMachine.GoToNextMode);

   public string   Content       { get; set; }
   public string   Description   { get; set; }
   public string   Title         { get; set; }
}
```

#### Add some View Models

These are hyper-simple; the base class does everything for them. Notice that the constructors ask for services. This demonstrates that the baton-passs from the **app.xaml.cs** DI container down to our own DI container has been successful.

``` C#   
public class ViewModel_Global : CustomViewModelBase, IViewModel_Global
{
   public ViewModel_Global(IGlobalServiceTwo service2, IGlobalServiceThree service3) {}
}

public class ViewModel_Private : CustomViewModelBase, IViewModel_Private
{
   public ViewModel_Private(IGlobalServiceOne service1, IGlobalServiceThree service3) {}
}

public class ViewModel_ToBeShared : CustomViewModelBase, IViewModel_ToBeShared
{
   public ViewModel_ToBeShared(IGlobalServiceTwo service2) {}
}
```

#### Create a Page to Display the View Model Data

Your UI will be a lot more complicated. This sample shows how to use a single page to display various view models. Remember: in real C# programing, there is no **_forced alignment_** between a page/view and its view model. That is set **_dynamically_** at _**run-time**_ based on **_live conditions and business logic_**.

This page uses the life-cycle aware **[ContentPageWithLifecycle](https://marcusts.com/2018/05/01/taking-control-of-variable-lifecycle/)**, which is highly recommended.

``` C#
<?xml version="1.0" encoding="utf-8" ?>
<pages:ContentPageWithLifecycle
   xmlns="http://xamarin.com/schemas/2014/forms"
   xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
   xmlns:pages="clr-namespace:Com.MarcusTS.LifecycleAware.Views.Pages;assembly=Com.MarcusTS.LifecycleAware"
   x:Class="Com.MarcusTS.SmartDI.LifecycleAware.SampleApp.Views.GeneralPage">
   <pages:ContentPageWithLifecycle.Content>
        <StackLayout
           VerticalOptions="CenterAndExpand"
           HorizontalOptions="CenterAndExpand"
           Margin="50"
           Spacing="25">
           <Label
              Text="{Binding Title}"
              FontSize="32"
              FontAttributes="Bold"
              VerticalOptions="Center"
              HorizontalOptions="Center"
              VerticalTextAlignment="Center"
              HorizontalTextAlignment="Center" />
           <Label
              Text="{Binding Description}"
              FontSize="18"
              FontAttributes="Italic"
              VerticalOptions="Center"
              HorizontalOptions="Center"
              VerticalTextAlignment="Center"
              HorizontalTextAlignment="Center" />
         <Label
              Text="{Binding Content}"
              FontSize="32"
              TextColor="DarkGreen"
              FontAttributes="Bold"
              VerticalOptions="Center"
              HorizontalOptions="Center"
              VerticalTextAlignment="Center"
              HorizontalTextAlignment="Center" />
           <Button
              Command="{Binding ButtonCommand}"
              BackgroundColor="DarkBlue"
              WidthRequest="100"
              HeightRequest="50"
              Text="NEXT"
              FontSize="18"
              TextColor="White"
              FontAttributes="Bold"
              VerticalOptions="Center"
              HorizontalOptions="Center" />
        </StackLayout>
    </pages:ContentPageWithLifecycle.Content>
</pages:ContentPageWithLifecycle>
```

#### Create a State Machine to Navigate and to Determine What Page goes with What View Model at that Instant

``` C#   public static class StateMachine
   {
      public enum PageModes
      {
         Private_1,
         Private_2,
         Shared_1,
         Shared_2,
         Global_1,
         Global_2,
         END
      }

      static StateMachine()
      {
         _privateViewModel1.Title = "Private View Model #1";
         _privateViewModel1.Description =
            "This view model's content is separate from any other view model.  It is never stored globally.";
         _privateViewModel1.Content = "C A T";

         _privateViewModel2.Title = "Private View Model #2";
         _privateViewModel2.Description =
            "This view model's content is also private, but the content has changed.  It is completely different from the previous view model.";
         _privateViewModel2.Content = "C H A I R";

         _sharedViewModel1.Description =
            "This view model is shared, so no matter how many copies we Resolve they are always the same.  They are stored globally until their parent pages fall out of scope.";
         _sharedViewModel1.Content = "H O U S E";

         // No need to set _sharedViewModel2; it is the same memory reference as _sharedViewModel1

         _globalViewModel1.Description =
            "This view model is global, so, like shared, no matter how many copies we Resolve they are always the same.  They are stored globally for the life of the container, which, in this case, is the life olf the app.";
         _globalViewModel1.Content = "W A T E R";

         // No need to set _globalViewModel2; it is the same memory reference as _globalViewModel1
         _endViewModel.Title       = "END";
         _endViewModel.Description = "To restart, click 'NEXT'";
      }

      private static PageModes CurrentPageMode
      {
         get => _currentPageMode;
         set
         {
            _currentPageMode = value;

            switch (_currentPageMode)
            {
               case PageModes.Private_1:
                  SetMainPage(_generalPage1, _privateViewModel1);
                  break;

               case PageModes.Private_2:
                  SetMainPage(_generalPage2, _privateViewModel2);
                  break;

               case PageModes.Shared_1:
                  // We over-write the shared title for the sake of clarity
                  _sharedViewModel1.Title = "Shared View Model #1";
                  SetMainPage(_generalPage1, _sharedViewModel1);
                  break;

               case PageModes.Shared_2:
                  // We over-write the shared title for the sake of clarity
                  _sharedViewModel2.Title = "Shared View Model #2";
                  SetMainPage(_generalPage2, _sharedViewModel2);
                  break;

               case PageModes.Global_1:
                  // We over-write the global title for the sake of clarity
                  _globalViewModel1.Title = "Global View Model #1";
                  SetMainPage(_generalPage1, _globalViewModel1);
                  break;

               case PageModes.Global_2:
                  // We over-write the global title for the sake of clarity
                  _globalViewModel1.Title = "Global View Model #2";
                  SetMainPage(_generalPage2, _globalViewModel2);
                  break;

               default:
                  SetMainPage(_generalPage1, _endViewModel);
                  break;
            }
         }
      }

      private static void SetMainPage(ContentPage             page,
                                      IViewModelWithLifecycle viewModel)
      {
         page.BindingContext          = viewModel;
         Application.Current.MainPage = page;
      }

      public static void GoToNextMode()
      {
         if ((int) CurrentPageMode < Enum.GetValues(typeof(PageModes)).Length - 1)
         {
            CurrentPageMode = (PageModes) ((int) CurrentPageMode + 1);
         }
         else
         {
            ResetCurrentPageMode();
         }
      }

      public static void ResetCurrentPageMode()
      {
         CurrentPageMode = 0;
      }

      private static PageModes _currentPageMode;

      private static readonly ICustomViewModelBase _endViewModel = ViewModelUtils.ViewModelBuilder.CreateViewModel<IViewModel_Private>();
      private static readonly ContentPage _generalPage1 = new GeneralPage();
      private static readonly ContentPage _generalPage2 = new GeneralPage();
      private static readonly ICustomViewModelBase _globalViewModel1 = ViewModelUtils.ViewModelBuilder.CreateViewModel<IViewModel_Global>();
      private static readonly ICustomViewModelBase _globalViewModel2 = ViewModelUtils.ViewModelBuilder.CreateViewModel<IViewModel_Global>();
      private static readonly ICustomViewModelBase _privateViewModel1 = ViewModelUtils.ViewModelBuilder.CreateViewModel<IViewModel_Private>();
      private static readonly ICustomViewModelBase _privateViewModel2 = ViewModelUtils.ViewModelBuilder.CreateViewModel<IViewModel_Private>();
      private static readonly ICustomViewModelBase _sharedViewModel1 = ViewModelUtils.ViewModelBuilder.CreateSharedViewModel<IViewModel_ToBeShared>(_generalPage1);
      private static readonly ICustomViewModelBase _sharedViewModel2 = ViewModelUtils.ViewModelBuilder.CreateSharedViewModel<IViewModel_ToBeShared>(_generalPage2);
   }
```

#### Start it Up

From the earlier code in this article, we start the app by asking the **State Machine** to reset:

``` C#      public App()
      {
         // code omitted ...

         **StateMachine.ResetCurrentPageMode();**
      }
```

The **State Machine** resolves view models from the **View Model Factory**. They work because Resolve() retrieves the view model as it was registered. If you run the sample app, you can see each type of view model and read the description about if or how it is stored.

## See the Source

It's all available for free on [GitHub](https://github.com/marcusts/Com.MarcusTS.SmartDI).

To bring the Smart DI Container into your own app, include these NuGet packages:
[Com.MarcusTS.SmartDI](https://www.nuget.org/packages/Com.MarcusTS.SmartDI/) 
[Com.MarcusTS.SmartDI.Lifecycle](https://www.nuget.org/packages/Com.MarcusTS.SmartDI.LifecycleAware/) 
[Com.MarcusTS.LifecycleAware](https://www.nuget.org/packages/Com.MarcusTS.LifecycleAware/)

## _Appendix:_ Living Without the Lifecycle Aware Guidance

If you simply cannot use the **Lifecycle Aware Guidance** for any reason, the **Smart DI Container** is still the best DI Container you can leverage, for all, of the reasons stated here. Here is how you can to proceed without the complete guidance:

1.  Include the Smart Di Container's basic Nuget: [Com.MarcusTS.SmartDI](https://www.nuget.org/packages/Com.MarcusTS.SmartDI/)
2.  You must notify the container whenever a bound parent loses scope. That is because it is linked to an object that is stored inside the container. Remember the steps as laid out in this article:

    *   You Register an object _(such as a view model)_ that you wish to share by specifying the **Storage Rule** as **SharedDependencyBetweenInstances**
    *   You Resolve the instance out of the container, sending along the parent that you wish to link to the instance. For a view model you, probably want to link the Page that acts as its parent.
    *   When the parent dies, the view model should be removed from the container. But without the **Lifecycle Aware Guidance**, you have to do this yourself.
3.  Whenever a bound parent dies, and you want its "child" view model to be removed from the container, call this **SmartDIContainer** method: **ContainerObjectIsDisappearing(object containerObj)**. Pass in the parent that is now out of scope. Do _**not**_ pass in the view model that is being stored.
4.  The **Smart DI Container** is "smart" enough to realize that the view model is being orphaned, so will remove it automatically.
