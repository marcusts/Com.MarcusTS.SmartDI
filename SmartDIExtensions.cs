// *********************************************************************************
// <copyright file=SmartDIExtensions.cs company="Marcus Technical Services, Inc.">
//     Copyright @2019 Marcus Technical Services, Inc.
// </copyright>
// 
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// *********************************************************************************

namespace Com.MarcusTS.SmartDI
{
   using System;
   using System.Collections.Concurrent;
   using System.Collections.Generic;
   using System.Linq;
   using Com.MarcusTS.SharedUtils.Utils;

   /// <summary>
   ///    Class SmartDIExtensions.
   /// </summary>
   public static class SmartDIExtensions
   {
      #region Public Methods

      /// <summary>
      ///    Adds the or update.
      /// </summary>
      /// <param name="retDict">The ret dictionary.</param>
      /// <param name="key">The key.</param>
      /// <param name="value">The value.</param>
      public static void AddOrUpdate
      (
         this ConcurrentDictionary<Type, ITimeStampedCreatorAndStorageRules> retDict,
         Type                                                                key,
         ITimeStampedCreatorAndStorageRules                                  value
      )
      {
         retDict.AddOrUpdate(key, value,
                             (
                                k,
                                v
                             ) => v);
      }

      /// <summary>
      ///    Attempts to resolve, and on failure, registers and tries again.
      ///    Might still return null, so must be checked after the call against default{classT}.
      /// </summary>
      /// <typeparam name="classT">The type of the class t.</typeparam>
      /// <param name="diContainer">The di container.</param>
      /// <param name="storageRule">The storage rule.</param>
      /// <param name="boundInstance">The bound instance.</param>
      /// <param name="creator">The creator.</param>
      /// <param name="conflictResolver">The conflict resolver.</param>
      /// <returns>classT.</returns>
      public static classT RegisterAndResolve<classT>
      (
         this ISmartDIContainer                                                           diContainer,
         StorageRules                                                                     storageRule      = StorageRules.AnyAccessLevel,
         object                                                                           boundInstance    = null,
         Func<object>                                                                     creator          = null,
         Func<IDictionary<Type, ITimeStampedCreatorAndStorageRules>, IConflictResolution> conflictResolver = null
      )
         where classT : class
      {
         var triedOnce = false;

         TRYAGAIN:

         // Prevents thrown errors temporarily
         diContainer.IgnoreAllErrors = true;

         // Resolves CLASS
         var possibleT = diContainer.Resolve<classT>(storageRule, boundInstance, conflictResolver);

         diContainer.IgnoreAllErrors = false;

         if (possibleT.IsNotAnEqualObjectTo(default(classT)))
         {
            return possibleT;
         }

         // No reason to keep trying
         if (triedOnce)
         {
            return default(classT);
         }

         // ELSE Register and try again
         RegisterSoloType<classT>(diContainer, storageRule, creator);
         triedOnce = true;
         goto TRYAGAIN;
      }

      /// <summary>
      ///    Attempts to resolve, and on failure, registers and tries again.
      ///    Might still return null, so must be checked after the call against default{classT}.
      /// </summary>
      /// <typeparam name="classT">The type of the class t.</typeparam>
      /// <typeparam name="interfaceT">The type of the interface t.</typeparam>
      /// <param name="diContainer">The di container.</param>
      /// <param name="storageRule">The storage rule.</param>
      /// <param name="boundInstance">The bound instance.</param>
      /// <param name="creator">The creator.</param>
      /// <param name="conflictResolver">The conflict resolver.</param>
      /// <returns>interfaceT.</returns>
      public static interfaceT RegisterAndResolveAsInterface<classT, interfaceT>
      (
         this ISmartDIContainer                                                           diContainer,
         StorageRules                                                                     storageRule      = StorageRules.AnyAccessLevel,
         object                                                                           boundInstance    = null,
         Func<object>                                                                     creator          = null,
         Func<IDictionary<Type, ITimeStampedCreatorAndStorageRules>, IConflictResolution> conflictResolver = null
      )
         where classT : class, interfaceT
         where interfaceT : class
      {
         var triedOnce = false;

         TRYAGAIN:

         // Prevents thrown errors temporarily
         diContainer.IgnoreAllErrors = true;

         // Resolves INTERFACE
         var possibleT = diContainer.Resolve<interfaceT>(storageRule, boundInstance, conflictResolver);

         diContainer.IgnoreAllErrors = false;

         if (possibleT.IsNotAnEqualObjectTo(default(classT)))
         {
            return possibleT;
         }

         // No reason to keep trying
         if (triedOnce)
         {
            return default(classT);
         }

         // ELSE Register and try again
         RegisterTypeAsInterface<classT>(diContainer, typeof(interfaceT), storageRule, creator);
         triedOnce = true;
         goto TRYAGAIN;
      }

      /// <summary>
      ///    Another easy-access call to <see cref="RegisterType" />.
      /// </summary>
      /// <param name="diContainer">The di container.</param>
      /// <param name="classType">Type of the class.</param>
      /// <param name="storageRule">The storage rule.</param>
      /// <param name="creator">The creator.</param>
      public static void RegisterSoloType
      (
         this ISmartDIContainer diContainer,
         Type                   classType,
         StorageRules           storageRule = StorageRules.AnyAccessLevel,
         Func<object>           creator     = null
      )
      {
         diContainer.RegisterType(classType, storageRule, creator, true);
      }

      /// <summary>
      ///    The same as <see cref="RegisterType" />, but with hyper-simplified parameters.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="diContainer">The di container.</param>
      /// <param name="storageRule">The storage rule.</param>
      /// <param name="creator">The creator.</param>
      public static void RegisterSoloType<T>
      (
         this ISmartDIContainer diContainer,
         StorageRules           storageRule = StorageRules.AnyAccessLevel,
         Func<object>           creator     = null
      )
         where T : class
      {
         diContainer.RegisterSoloType(typeof(T), storageRule, creator);
      }

      /// <summary>
      ///    Registers a base type so that it can be resolved in the future.
      ///    Most of the parameters are optional. If omitted, we make the exact class type
      ///    available for a call to Resolve(), but do not allow other forms of access.
      ///    To Resolve() and convert to an interface, add those to typesToCastAs.
      ///    You can also call the base method, which is wide open but must be
      ///    managed carefully.
      /// </summary>
      /// <param name="diContainer">The DI container -- omitted when you call this method, as it is an extension.</param>
      /// <param name="classType">The base class type that will be constructed. *Must* be a concrete class.</param>
      /// <param name="storageRule">
      ///    Determines if a strict rule will be enforced about how the new instance of the class will be stored upon Resolve():
      ///    * AnyAccessLevel: The default; allows the caller to Resolve to determine the way the instance will be stored.
      ///    * DoNotStore: A new instance of the variable will be issued, but it will *not* be stored.
      ///    This is typical where you just need a view model for a view, and there is no reason to maintain it globally.
      ///    If the view model contains data, however, and if it might be required elsewhere in the app, then this will
      ///    cause you to have isolated instances that have separate states. So in that case, it is not recommended.
      ///    You should use the SharedDependencyBetweenInstances in that case.
      ///    * SharedDependencyBetweenInstances: The container will issue an instance and also store it.
      ///    It will be shared with any requester. This *requires* that you supply your host ("bound") class,
      ///    as there is no other way to manage the relationship between that host and this new instance.
      ///    For example, if you bind a view model to a view, the host is the view and the resolved instance is the view model.
      ///    In the same scenario, the view might belong to a page, so the host would be the page
      ///    and the resolved instance will be the view.
      ///    Because it is a shared instance, it cannot be considered "private".  If that is required, use an DoNotStore.
      ///    As soon as all of the bound hosts are disposed, this instance will also be automatically removed from the container.
      ///    * GlobalSingleton: Creates only one instance of the requested type and stores it globally *forever* as long as the
      ///    container is alive.
      ///    Can be used for service injection. Almost never used for any other purpose.
      /// </param>
      /// <param name="creator">
      ///    A function for creating the class type, if any.
      ///    You do not need to cast as the final type.  The container handles that for you.
      /// </param>
      /// <param name="addMainTypeAsDefault">
      ///    Optional, and defaulted to false.  Often, DI containers are asked to create these sorts of instances:
      ///    * Cat as IAnimal
      ///    * Dog as IAnimal
      ///    * Bird as IAnimal
      ///    In all three cases, you might pass this as the creator: "() =&gt; new Cat()" or dog or bird, etc.
      ///    We would then typecast the resulting instance as IAnimal for you.
      ///    However, you might do something entirely different:
      ///    * MyClass as ImplementedInterface
      ///    In which case, your creator might be: "() =&gt; new MyClass()".  We would resolve this as ImplementedInterface.
      ///    But what if you also wanted to resolve like this: "Resolve{BaseClass}();" ???
      ///    You would turn this boolean parameter to True.
      ///    Your registration would be:  RegisterType(BaseClass, creator: () =&gt; new BaseClass, addMainAsDefault =
      ///    true,typesToCastAs = typeof(ImplementedInterface).
      ///    After that, you can resolve as either BaseClass or ImplementedInterface.
      /// </param>
      /// <param name="typesToCastAs">
      ///    The list of types to type-cast the constructed base class as.
      ///    It can be any number. Remember to use "typeof(your type)" for each type, separated by a comma.
      ///    The storage rule for each of these types is the same as the main one you pass in.
      ///    The creator will also be the same for all of these types.
      ///    To create more complex storage rules and creators, call the main library's
      ///    <see cref="SmartDIContainer.RegisterTypeContracts" />.
      /// </param>
      public static void RegisterType
      (
         this ISmartDIContainer diContainer,
         Type                   classType,
         StorageRules           storageRule          = StorageRules.AnyAccessLevel,
         Func<object>           creator              = null,
         bool                   addMainTypeAsDefault = false,
         params Type[]          typesToCastAs
      )
      {
         // If accessible types contains the main class type, it is the same as setting that Boolean true
         addMainTypeAsDefault = addMainTypeAsDefault || typesToCastAs.Any(i => i == classType);

         var dictToAdd = new Dictionary<Type, IProvideCreatorAndStorageRule>();

         if (addMainTypeAsDefault)
         {
            dictToAdd.Add(classType, new CreatorAndStorageRule(creator, storageRule));
         }

         var eligibleRulesAndCreators = typesToCastAs.Where(i => i != classType);

         if (eligibleRulesAndCreators.IsNotEmpty())
         {
            foreach (var interfaceType in typesToCastAs)
            {
               // We add these with the same constructor and storage rules.
               dictToAdd.Add(interfaceType, new CreatorAndStorageRule(creator, storageRule));
            }
         }

         // ClassT may or may not be included in the resolvable types.
         // *However*, if there are no resolvable types (no 'creators and rules'), then we force ClassT to be resolvable.
         // Otherwise, this is a meaningless registration that will always fail.
         if (dictToAdd.IsEmpty())
         {
            // Will auto-instantiate ClassT using Activator.CreateInstance.
            dictToAdd.Add(classType, new CreatorAndStorageRule(creator, storageRule));
         }

         diContainer.RegisterTypeContracts(classType, dictToAdd);
      }

      /// <summary>
      ///    The same as <see cref="RegisterType" />, except with a more Generic way to state the base class type.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="diContainer">The di container.</param>
      /// <param name="storageRule">The storage rule.</param>
      /// <param name="creator">The creator.</param>
      /// <param name="addMainTypeAsDefault">if set to <c>true</c> [add main type as default].</param>
      /// <param name="typesToCastAs">The types to cast as.</param>
      public static void RegisterType<T>
      (
         this ISmartDIContainer diContainer,
         StorageRules           storageRule          = StorageRules.AnyAccessLevel,
         Func<object>           creator              = null,
         bool                   addMainTypeAsDefault = false,
         params Type[]          typesToCastAs
      )
         where T : class
      {
         diContainer.RegisterType(typeof(T), storageRule, creator, addMainTypeAsDefault, typesToCastAs);
      }

      /// <summary>
      ///    Another easy-access call to <see cref="RegisterType" />.
      /// </summary>
      /// <param name="diContainer">The di container.</param>
      /// <param name="classType">Type of the class.</param>
      /// <param name="interfaceType">Type of the interface.</param>
      /// <param name="storageRule">The storage rule.</param>
      /// <param name="creator">The creator.</param>
      public static void RegisterTypeAsInterface
      (
         this ISmartDIContainer diContainer,
         Type                   classType,
         Type                   interfaceType,
         StorageRules           storageRule = StorageRules.AnyAccessLevel,
         Func<object>           creator     = null
      )
      {
         diContainer.RegisterType(classType, storageRule, creator, false, interfaceType);
      }

      /// <summary>
      ///    The same as <see cref="RegisterType" />, but with hyper-simplified parameters.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="diContainer">The di container.</param>
      /// <param name="interfaceType">Type of the interface.</param>
      /// <param name="storageRule">The storage rule.</param>
      /// <param name="creator">The creator.</param>
      public static void RegisterTypeAsInterface<T>
      (
         this ISmartDIContainer diContainer,
         Type                   interfaceType,
         StorageRules           storageRule = StorageRules.AnyAccessLevel,
         Func<object>           creator     = null
      )
         where T : class
      {
         diContainer.RegisterTypeAsInterface(typeof(T), interfaceType, storageRule, creator);
      }

      /// <summary>
      ///    Provides a generic argument for the type to resolve.
      ///    Casts as the requested type upon return.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="diContainer">The di container.</param>
      /// <param name="storageRule">The storage rule.</param>
      /// <param name="boundInstance">The bound instance.</param>
      /// <param name="conflictResolver">The conflict resolver.</param>
      /// <returns>T.</returns>
      public static T Resolve<T>
      (
         this ISmartDIContainer                                                           diContainer,
         StorageRules                                                                     storageRule      = StorageRules.AnyAccessLevel,
         object                                                                           boundInstance    = null,
         Func<IDictionary<Type, ITimeStampedCreatorAndStorageRules>, IConflictResolution> conflictResolver = null
      )
         where T : class
      {
         var retInstance = diContainer.Resolve(typeof(T), storageRule, boundInstance, conflictResolver) as T;
         return retInstance;
      }

      #endregion Public Methods
   }
}