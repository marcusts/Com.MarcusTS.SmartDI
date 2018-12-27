## Finally, a DI Container Without The Fake "IOC" Swagger

#### _It's just dependency injection - that's all, folks_

Modern programmers require some means of creating new classes out of other classes that are passed as parameters, and to do so in an elegant, seamless manner. Those new instances often require _caching_, especially if they are services or view models. The got-to solution for this challenge has been the mis-named and grossly mis-described _"IOC Container"_. In [another article](https://marcusts.com/2018/04/09/the-ioc-container-anti-pattern/), I explained why that is a bad idea. If we can't even name something accurately, it is time to reconsider it entirely. So I have done that here with the tool that all of us actually need: a DI _("Dependency Injection")_ Container.

## What's So Smart About It

#### 1\. It provides true _**lifecycle management**_

IOC containers always store an instance when you create it. That is extremely _**wasteful**_. It is also _**unsafe**_. The **Smart DI Container** provides only three types of registrations:

*   **Any Access Level:** Use this to create instances at any time and without caching. The local variable you retrieve on Resolve() is the only stored reference.

![](https://marcusts.com/wp-content/uploads/2018/12/AnyAccessLevel1.png)

*   **Shared Between Instances** When you Resolve() using this access level, you must to pass in a "parent" object that gets indexed to that new class instance. You can also link the same instance to any number of other consumers/parents by calling Resolve<> again. Once all of the parents die, the cached instance is also removed. _Note: This requires you to raise a Xamarin.Forms event to notify the container about the death of a shared instance parent. We do this for you if you also use our [Lifecycle Aware Library](https://marcusts.com/2018/05/01/taking-control-of-variable-lifecycle/)._

![](https://marcusts.com/wp-content/uploads/2018/12/SharedAccessLevel.png)

*   **Global Singleton:** The container creates and caches a permanent instance of any type registered with this access level. The cached reference dies when the container itself falls out of scope.

![](https://marcusts.com/wp-content/uploads/2018/12/GlobalSingleton.png)

In spite of the ginormous size of other containers on the market, none of them can pass this test. The container must provide a **_physical mechanism_** to make this functionality possible. We have one!

#### 2\. It is not global or static

You can declare an instance of the **Smart DI Container** wherever you please. This supports "nested" scenarios, where containers live within narrowly defined class inheritance trees. **_Remember:_** all "global" variables stored/cached will only live as long as the container does.

#### 3\. It is well-behaved

The **Smart DI Container** protects against recursive calls, or any other violation of the rules-based registrations you make. For instance, if you register two competing interfaces for the same base type:

<pre class="prettyprint lang-javascript" data-start-line="1" data-visibility="visible" data-highlight="" data-caption="">_container = new SmartDIContainer();
_container.RegisterTypeAsInterface<FirstSimpleClass>(typeof(IAmSimple));
_container.RegisterTypeAsInterface<SecondSimpleClass>(typeof(IAmSimple));
</pre>

... and then resolve IAmSimple, you have created a _**conflict**_. The container cannot know which one to return. You can set a Boolean property to throw an error in this case. Or you can provide a "conflict resolver":

<pre class="prettyprint lang-javascript" data-start-line="1" data-visibility="visible" data-highlight="" data-caption="">var simple = _container.Resolve<IAmSimple>(StorageRules.AnyAccessLevel, null, ForbidSpecificClass<FirstSimpleClass>);

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
      }</pre>

#### 4\. It is **_tiny_**

**Smart DI Container** occupies almost no space at all, and rarely touches memory, since it does not store anything unnecessarily.

#### 5\. It's basic, honest open source C#, and easy to read.

We actually added comments! _(And we were not struck by lightning)_

#### 6\. It is tested and proven

See the [unit tests](https://github.com/marcusts/SafeDiContainer).

## Quick Start

#### Create DI Containers Wherever You Need Them; Do Not Centralize

DI Containers both register and provide access to variables. To stay within the C# SOLID Guidance, your app should be as _**private as possible**_. So the last thing you need are global containers. Perhaps services can be stored in such a manner. But not your view models. Consider using a factory that generates view models and regulates what gets stored in the container and what is just instantiated out-right:

<pre class="prettyprint lang-javascript" data-start-line="1" data-visibility="visible" data-highlight="" data-caption="">public interface IViewModel <t>{}

public interface IViewModelFactory
{
   IViewModel CreateSimpleViewModel<T>();
   IViewModel CreateGlobalViewModel<T>();
   IViewModel CreateSharedViewModel<T>(object o);
}

public class ViewModelFactory : IViewModelFactory
{
   private SmartDIContainer _viewModelContainer = new SmartDIContainer();

   public ViewModelFactory(IService service)
   {
      // Perform registrations at the constructor, privately.

      // Can pass in services to seed this container or can Resolve this factory from another, more global container
      _smartDIContainer.RegisterTypeAsInterface<SomeService>(typeof(IService), StorageRules.AnyAccessLevel, o => service);

      // Register other known types using various access levels
      _smartDIContainer.RegisterTypeAsInterface<SomeClass>(typeof(ISomeClass));
      _smartDIContainer.RegisterTypeAsInterface<GlobalClass>(typeof(IGlobalClass), StorageRules.GlobalSingleton);
      _smartDIContainer.RegisterTypeAsInterface<SharedClass>(typeof(ISharedClass), StorageRules.SharedDependencyBetweenInstances);
   }

   public IViewModel CreateSimpleViewModel<T>()
   {
      return _container.Resolve<T>();
   }

   public IViewModel CreateGlobalViewModel()
   {
      return _container.Resolve<T>(StorageRules.GlobalSingleton);
   }

   public IViewModel CreateSharedViewModel(object o)
   {
      return _container.Resolve<T>(StorageRules.SharedDependencyBetweenInstances, o);
   }
}</t></pre>

#### Implement the LifecycleAware Guidance

You can easily make your Xamarin.Forms pages, views, and view models compliant with lifecycle management. See the [description and code samples](https://marcusts.com/2018/05/01/taking-control-of-variable-lifecycle/). If you already have your own base classes, you can copy a few snippets of code from the base class examples in the LifecycleAware library into those existing files. If that is too much trouble, you can still use the Smart DI Container's "Any" and "Global" access levels, which cover most cases for smaller apps.

