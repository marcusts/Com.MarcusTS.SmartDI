// *********************************************************************************
// Assembly         : Com.MarcusTS.SmartDI
// Author           : Stephen Marcus (Marcus Technical Services, Inc.)
// Created          : 05-05-2018
// Last Modified On : 12-27-2018
//
// <copyright file="SmartDIContainer.cs" company="Marcus Technical Services, Inc.">
//     @2018 Marcus Technical Services, Inc.
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
   using System.Diagnostics;
   using System.Linq;
   using SharedUtils.Utils;

   /// <summary>
   /// Enum StorageRules
   /// </summary>
   public enum StorageRules
   {
      /// <summary>
      ///    Default for class management; instantiates but does not store.
      /// </summary>
      /// <summary>
      /// Any access level
      /// </summary>
      AnyAccessLevel,

      /// <summary>
      ///    Requires one or more companion "parents"; once all shared parents are gone, the stored instance is removed.
      /// </summary>
      /// <summary>
      /// The shared dependency between instances
      /// </summary>
      SharedDependencyBetweenInstances,

      /// <summary>
      ///    Is stored once requested, and thereafter remains in memory for the life of the container.
      /// </summary>
      /// <summary>
      /// The global singleton
      /// </summary>
      GlobalSingleton,

      /// <summary>
      ///    The instance is not stored after Resolve.
      ///    In the "any" case, the resolver can ask for AnyAccessLevel and can then resolve as Any, Shared, or Singleton without
      ///    error.
      ///    In this case, the resolver *must* ask for DoNotStore as the requested access level.
      ///    Use this access level if you are 100% certain that you never want to store the type as registered.
      /// </summary>
      /// <summary>
      /// The do not store
      /// </summary>
      DoNotStore
   }

   /// <summary>
   ///    Interface ISmartDIContainer
   ///    Implements the <see cref="System.IDisposable" />
   /// </summary>
   /// <seealso cref="System.IDisposable" />
   /// <seealso cref="System.IDisposable" />
   /// <summary>
   /// Interface ISmartDIContainer
   /// Implements the <see cref="System.IDisposable" />
   /// </summary>
   /// <seealso cref="System.IDisposable" />
   public interface ISmartDIContainer : IDisposable
   {
      #region Public Methods

      /// <summary>
      ///    Called by the deriver whenever a class is about to disappear from view. It is better to call this before the
      ///    finalizer, as that can be extremely late. An example would be Xamarin.Forms.ContentPage.OnDisappearing. Other
      ///    views or view models will have to listen to the original page event and then notify about their own demise. If
      ///    this step is skipped, none of the lifecycle protections will occur!
      /// </summary>
      /// <param name="containerObj">The container object.</param>
      /// <summary>
      /// Containers the object is disappearing.
      /// </summary>
      /// <param name="containerObj">The container object.</param>
      void ContainerObjectIsDisappearing(object containerObj);

      /// <summary>
      ///    Determine of a qualifying registration exists for a given type.
      ///    Could be used as a pre-tst before Resolve() if confusion exists as to the safety.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <returns><c>true</c> if a qualifying registration exits, else <c>false</c>.</returns>
      /// <summary>
      /// Qualifyings the registrations exist.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
      bool QualifyingRegistrationsExist<T>();

      /// <summary>
      ///    Determine of a qualifying registration exists for a given type.
      ///    Could be used as a pre-tst before Resolve() if confusion exists as to the safety.
      /// </summary>
      /// <param name="type">The class type that would be instantiated by the qualifying registration.</param>
      /// <returns><c>true</c> if a qualifying registration exits, else <c>false</c>.</returns>
      /// <summary>
      /// Qualifyings the registrations exist.
      /// </summary>
      /// <param name="type">The type.</param>
      /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
      bool QualifyingRegistrationsExist(Type type);

      /// <summary>
      ///    Adds a list of types that the type can be resolved as. Includes creators and storage rules.
      /// </summary>
      /// <param name="classT">The class type that owns the contracts.</param>
      /// <param name="creatorsAndRules">The list of class creators and rules. The creators can be null.</param>
      /// <summary>
      /// Registers the type contracts.
      /// </summary>
      /// <param name="classT">The class t.</param>
      /// <param name="creatorsAndRules">The creators and rules.</param>
      void RegisterTypeContracts(Type                                             classT,
                                 IDictionary<Type, IProvideCreatorAndStorageRule> creatorsAndRules);

      /// <summary>
      ///    Creates an instance of a class and stores it according to the requested rules. Only works if you have
      ///    registered each base class first along with any interface they should be available (type-cast) as. See
      ///    <see cref="RegisterTypeContracts" /> for details about this.
      /// </summary>
      /// <param name="typeRequestedT">
      ///    This is the type that you wish to receive. It is not necessarily the "base" class type. It is more commonly an
      ///    interface implemented by your base class type.
      /// </param>
      /// <param name="storageRule">
      ///    This is a *request* for a storage rule, but is subject to strict guidelines:
      ///    * If you ask for "AnyAccessLevel", and there is no other value set in the registration, we will give you an
      ///    unstored local variable.
      ///    * If, when you registered, you set the access level to something like "SharedDependencyBetweenInstances", and
      ///    then here on Resolve ask for "GlobalSingleton", we default to throw with an illegal request.
      /// </param>
      /// <param name="boundInstance">
      ///    The "host" or "bound" class that is attached to this instance. Only required if you need a
      ///    "SharedDependencyBetweenInstances".
      /// </param>
      /// <param name="conflictResolver">
      ///    This is an advanced parameter where you can include a function to "break the tie" when this container tries to
      ///    Resolve, but comes up with more than one competing resolution contract. If you leave this at null, and we
      ///    cannot see a single legal choice, we will throw an error.
      /// </param>
      /// <returns>An object which *must* then be cast as the type requested by the *caller*.</returns>
      /// <summary>
      /// Resolves the specified type requested t.
      /// </summary>
      /// <param name="typeRequestedT">The type requested t.</param>
      /// <param name="storageRule">The storage rule.</param>
      /// <param name="boundInstance">The bound instance.</param>
      /// <param name="conflictResolver">The conflict resolver.</param>
      /// <returns>System.Object.</returns>
      object Resolve(Type         typeRequestedT,
                     StorageRules storageRule   = StorageRules.AnyAccessLevel,
                     object       boundInstance = null,
                     Func<ConcurrentDictionary<Type, ITimeStampedCreatorAndStorageRules>, IConflictResolution>
                        conflictResolver =
                        null);

      /// <summary>
      ///    Removes a list of types that the parent type can be resolved as. Includes creators and storage rules.
      /// </summary>
      /// <typeparam name="TParent">The generic parent type</typeparam>
      /// <param name="typesToUnregister">The types to remove.</param>
      /// <summary>
      /// Unregisters the type contracts.
      /// </summary>
      /// <typeparam name="TParent">The type of the t parent.</typeparam>
      /// <param name="typesToUnregister">The types to unregister.</param>
      void UnregisterTypeContracts<TParent>(params Type[] typesToUnregister);

      #endregion Public Methods
   }

   /// <summary>
   ///    Class SmartDIContainer.
   ///    Implements the <see cref="ISmartDIContainer" />
   ///    Implements the <see cref="Com.MarcusTS.SmartDI.ISmartDIContainer" />
   /// </summary>
   /// <seealso cref="Com.MarcusTS.SmartDI.ISmartDIContainer" />
   /// <seealso cref="ISmartDIContainer" />
   /// <seealso cref="Com.MarcusTS.SmartDI.ISmartDIContainer" />
   /// <summary>
   /// Class SmartDIContainer.
   /// Implements the <see cref="Com.MarcusTS.SmartDI.ISmartDIContainer" />
   /// </summary>
   /// <seealso cref="Com.MarcusTS.SmartDI.ISmartDIContainer" />
   public class SmartDIContainer : ISmartDIContainer
   {
      #region Public Constructors

      /// <summary>
      ///    Initializes a new instance of the <see cref="SmartDIContainer" /> class.
      /// </summary>
      /// <param name="throwOnMultipleRegisteredTypesForOneResolvedType">
      ///    if set to <c>true</c> [throw on multiple registered
      ///    types for one resolved type].
      /// </param>
      /// <param name="throwOnAttemptToAssignDuplicateContractSubType">
      ///    if set to <c>true</c> [throw on attempt to assign
      ///    duplicate contract sub type].
      /// </param>
      /// <summary>
      /// Initializes a new instance of the <see cref="SmartDIContainer"/> class.
      /// </summary>
      /// <param name="throwOnMultipleRegisteredTypesForOneResolvedType">if set to <c>true</c> [throw on multiple registered types for one resolved type].</param>
      /// <param name="throwOnAttemptToAssignDuplicateContractSubType">if set to <c>true</c> [throw on attempt to assign duplicate contract sub type].</param>
      public SmartDIContainer(bool throwOnMultipleRegisteredTypesForOneResolvedType = false,
                              bool throwOnAttemptToAssignDuplicateContractSubType   = false)
      {
         ThrowOnMultipleRegisteredTypesForOneResolvedType = throwOnMultipleRegisteredTypesForOneResolvedType;
         ThrowOnAttemptToAssignDuplicateContractSubType   = throwOnAttemptToAssignDuplicateContractSubType;
      }

      #endregion Public Constructors

      #region Private Destructors

      /// <summary>
      ///    Finalizes an instance of the <see cref="SmartDIContainer" /> class.
      /// </summary>
      /// <summary>
      /// Finalizes an instance of the <see cref="SmartDIContainer"/> class.
      /// </summary>
      ~SmartDIContainer()
      {
         Dispose(false);
      }

      #endregion Private Destructors

      #region Protected Fields

      /// <summary>
      ///    A dictionary of global singletons keyed by type. There can only be one each of a given type.
      /// </summary>
      /// <summary>
      /// The global singletons by type
      /// </summary>
      protected readonly IDictionary<Type, object> _globalSingletonsByType = new ConcurrentDictionary<Type, object>();

      /// <summary>
      ///    Specifies that a type can be resolved as a specific type (can be different as long as
      ///    related) Also sets the storage rules. Defaults to "all".
      /// </summary>
      /// <summary>
      /// The registered type contracts
      /// </summary>
      protected readonly IDictionary<Type, ITimeStampedCreatorAndStorageRules> _registeredTypeContracts =
         new ConcurrentDictionary<Type, ITimeStampedCreatorAndStorageRules>();

      /// <summary>
      ///    A dictionary of instances that are shared between one ore more other instances. When the list of shared
      ///    instances reaches zero, the main instance is removed.
      /// </summary>
      /// <summary>
      /// The shared instances with bound members
      /// </summary>
      protected readonly IDictionary<object, List<object>> _sharedInstancesWithBoundMembers =
         new ConcurrentDictionary<object, List<object>>();

      #endregion Protected Fields

      #region Protected Properties

      /// <summary>
      ///    Gets the is argument exception thrown.
      /// </summary>
      /// <value>The is argument exception thrown.</value>
      /// <summary>
      /// Gets the is argument exception thrown.
      /// </summary>
      /// <value>The is argument exception thrown.</value>
      protected string IsArgumentExceptionThrown { get; private set; }

      /// <summary>
      ///    Gets the is operation exception thrown.
      /// </summary>
      /// <value>The is operation exception thrown.</value>
      /// <summary>
      /// Gets the is operation exception thrown.
      /// </summary>
      /// <value>The is operation exception thrown.</value>
      protected string IsOperationExceptionThrown { get; private set; }

      /// <summary>
      ///    Gets or sets a value indicating whether this instance is unit testing.
      /// </summary>
      /// <value><c>true</c> if this instance is unit testing, else <c>false</c>.</value>
      /// <summary>
      /// Gets or sets a value indicating whether this instance is unit testing.
      /// </summary>
      /// <value><c>true</c> if this instance is unit testing; otherwise, <c>false</c>.</value>
      protected bool IsUnitTesting { get; set; }

      /// <summary>
      ///    If a user registers a contract like this: _container.RegisterType{SimpleClass}(StorageRules.DoNotStore, null,
      ///    false, typeof(IAmSimple)); _container.RegisterType{SimpleClass}(StorageRules.GlobalSingleton, null, false,
      ///    typeof(IAmSimple)); ... we have to save the second registration on top of the first one. Only one sub-type can
      ///    exist with a single storage level. With this Boolean set to false (default), we will casually over-write the
      ///    most recent entry on top of the old entry. With the value set to True, we will throw an error upon over-writing
      ///    any existing registration.
      /// </summary>
      /// <value><c>true</c> if [throw on attempt to assign duplicate contract sub type], else <c>false</c>.</value>
      /// <summary>
      /// Gets or sets a value indicating whether [throw on attempt to assign duplicate contract sub type].
      /// </summary>
      /// <value><c>true</c> if [throw on attempt to assign duplicate contract sub type]; otherwise, <c>false</c>.</value>
      protected bool ThrowOnAttemptToAssignDuplicateContractSubType { get; set; }

      /// <summary>
      ///    Registrations occur for base class types. They are usually delivered as interface types. We expect that when
      ///    you register an interface for Resolve, you will only have one interface for a given base class type.
      ///    So in this  example:
      ///    RegisterType(Class1, Interface1)
      ///    You would not also create:
      ///    RegisterType(Class2, Interface1)
      ///    This is considered to be unsafe because it is not clear or purposeful, and the container cannot make a refined
      ///    judgment with any known rules.
      ///    If this Boolean is set to True, we will throw an error if we come across such a condition.
      ///    To avoid the error, leave the Boolean at false, where it defaults.
      ///    With the setting false, we will  just pick the first available candidate.
      ///    THIS IS SLOPPY... so try to set this Boolean to True and make careful registrations.
      /// </summary>
      /// <value><c>true</c> if [throw on multiple registered types for one resolved type], else <c>false</c>.</value>
      /// <summary>
      /// Gets or sets a value indicating whether [throw on multiple registered types for one resolved type].
      /// </summary>
      /// <value><c>true</c> if [throw on multiple registered types for one resolved type]; otherwise, <c>false</c>.</value>
      protected bool ThrowOnMultipleRegisteredTypesForOneResolvedType { get; set; }

      #endregion Protected Properties

      #region Public Methods

      /// <summary>
      ///    Called by the deriver whenever a class is about to disappear from view. It is better to call this before the
      ///    finalizer, as that can be extremely late. An example would be Xamarin.Forms.ContentPage.OnDisappearing. Other
      ///    views or view models will have to listen to the original page event and then notify about their own demise. If
      ///    this step is skipped, none of the lifecycle protections will occur!
      /// </summary>
      /// <param name="containerObj">A variable that was inserted into the container "live" and is now being deactivated.</param>
      /// <summary>
      /// Called by the deriver whenever a class is about to disappear from view. It is better to call this before the
      /// finalizer, as that can be extremely late. An example would be Xamarin.Forms.ContentPage.OnDisappearing. Other
      /// views or view models will have to listen to the original page event and then notify about their own demise. If
      /// this step is skipped, none of the lifecycle protections will occur!
      /// </summary>
      /// <param name="containerObj">The container object.</param>
      public virtual void ContainerObjectIsDisappearing(object containerObj)
      {
         // Remove the class from the global singletons
         RemoveSingletonInstance(containerObj.GetType());

         // See if the shared instances contain the class. There are two possible scenarios:
         // 1. This class is the shared class. If so, we remove the entire node that contains the container class.

         // This only removes the object if it exists (its type is registered).
         RemoveSharedInstance(containerObj.GetType());

         // 2. This class is bound to a shared class. If true, we remove that single part of the node. However, if this
         //   is the last node, then we do as with #1 -- we kill the entire node.
         RemoveBoundSharedDependencies(containerObj);
      }

      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      // C O N T A I N E R   T Y P E   I S   D I S A P P E A R I N G
      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      /// <summary>
      ///    Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
      /// </summary>
      /// <summary>
      /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
      /// </summary>
      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      /// <summary>
      ///    Determine of a qualifying registration exists for a given type.
      ///    Could be used as a pre-tst before Resolve() if confusion exists as to the safety.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <returns><c>true</c> if a qualifying registration exits, else <c>false</c>.</returns>
      /// <summary>
      /// Determine of a qualifying registration exists for a given type.
      /// Could be used as a pre-tst before Resolve() if confusion exists as to the safety.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <returns><c>true</c> if a qualifying registration exits, else <c>false</c>.</returns>
      public bool QualifyingRegistrationsExist<T>()
      {
         return QualifyingRegistrationsExist(typeof(T));
      }

      /// <summary>
      ///    Determine of a qualifying registration exists for a given type.
      ///    Could be used as a pre-tst before Resolve() if confusion exists as to the safety.
      /// </summary>
      /// <param name="type">The class type that would be instantiated by the qualifying registration.</param>
      /// <returns><c>true</c> if a qualifying registration exits, else <c>false</c>.</returns>
      /// <summary>
      /// Determine of a qualifying registration exists for a given type.
      /// Could be used as a pre-tst before Resolve() if confusion exists as to the safety.
      /// </summary>
      /// <param name="type">The class type that would be instantiated by the qualifying registration.</param>
      /// <returns><c>true</c> if a qualifying registration exits, else <c>false</c>.</returns>
      public bool QualifyingRegistrationsExist(Type type)
      {
         return GetQualifyingRegistrations(type).IsNotEmpty();
      }

      /// <summary>
      ///    Adds a list of types that the type can be resolved as. Includes creators and storage rules.
      /// </summary>
      /// <param name="classT">The base type for the class rule.</param>
      /// <param name="creatorsAndRules">The list of class creators and rules. The creators can be null.</param>
      /// <summary>
      /// Adds a list of types that the type can be resolved as. Includes creators and storage rules.
      /// </summary>
      /// <param name="classT">The class type that owns the contracts.</param>
      /// <param name="creatorsAndRules">The list of class creators and rules. The creators can be null.</param>
      public void RegisterTypeContracts(Type                                             classT,
                                        IDictionary<Type, IProvideCreatorAndStorageRule> creatorsAndRules)
      {
         // ClassT must be a standard public type -- otherwise, can't be instantiated
         if (!classT.IsPublic || !classT.IsClass || classT.IsGenericType || classT.IsSealed || classT.IsAbstract ||
             classT.IsInterface)
         {
            ThrowArgumentException(nameof(RegisterTypeContracts),
                                   "Type ->" + classT + "<- must be a standard public type");
            return;
         }

         // ClassT may or may not be included in the resolvable types.
         // *However*, if there are no resolvable types (no 'creators and rules'), then we force ClassT to be resolvable by adding it here manually.
         //  Otherwise, this is a meaningless registration that will always fail.
         if (creatorsAndRules.IsEmpty())
         {
            // Add the new rule as the only member - order "0"
            creatorsAndRules = new ConcurrentDictionary<Type, IProvideCreatorAndStorageRule>();
            creatorsAndRules.Add(classT, new CreatorAndStorageRule());
         }
         else
         {
            // Interrogate the creatorsAdRules to make sure that all requested types can indeed be cast from classT.
            foreach (var creatorAndRule in creatorsAndRules)
            {
               if (!classT.IsTypeOrAssignableFromType(creatorAndRule.Key))
               {
                  ThrowArgumentException(nameof(RegisterTypeContracts),
                                         "Type ->" + classT + "<- cannot be cast as ->" +
                                         creatorAndRule.Key);
                  return;
               }
            }
         }

         // Are there other registrants that already return/resolve the any of the same classes as we do in our creators
         // and rules? If so, is it their master type the same as us (probably not unless we have duplicated this
         // registration exactly)?
         var creatorsAndRulesTypes = creatorsAndRules.Keys;
         var competingContracts = _registeredTypeContracts
                                 .Where(rc => rc.Key != classT && creatorsAndRulesTypes
                                                                 .Intersect(rc.Value.CreatorsAndStorageRules.Keys)
                                                                 .Any())
                                 .ToList();

         // If there is more than one master type, we should consider throwing, as this is extremely confusing for resolution.
         if (competingContracts.Any())
         {
            if (ThrowOnMultipleRegisteredTypesForOneResolvedType)
            {
               ThrowArgumentException(nameof(RegisterTypeContracts),
                                      "Cannot register a second contract when the property ->" +
                                      nameof(ThrowOnMultipleRegisteredTypesForOneResolvedType) +
                                      "< is true.  The competing contract is of type ->"       +
                                      competingContracts                                       +
                                      "<-");
               return;
            }
         }

         // All is good, so register

         // If the contract does not exist, add it and block-copy in the entire list of creators and rules
         if (!_registeredTypeContracts.ContainsKey(classT))
         {
            _registeredTypeContracts.Add(classT,
                                         new TimeStampedCreatorAndStorageRules
                                         {WhenAdded = DateTime.Now, CreatorsAndStorageRules = creatorsAndRules});
         }
         else
         {
            // Return the value to the array
            _registeredTypeContracts[classT] = SeekExistingContract(classT, creatorsAndRules);
         }
      }

      /// <summary>
      ///    Fetches an object of the requested type.
      ///    If the object does not exist, creates it.
      ///    In certain cases, stores the object.
      /// </summary>
      /// <param name="typeRequestedT">The type to cast the object as.  NOTE that the programmer does this after their Resolve.</param>
      /// <param name="ruleRequested">The sort of storage rule to use in managing the resolved object.</param>
      /// <param name="boundParent">
      ///    If the storage rule is <see cref="StorageRules.SharedDependencyBetweenInstances" />,
      ///    this is the object that will share the resolved object with other peers.
      /// </param>
      /// <param name="conflictResolver">
      ///    If supplied, determines which competing resolution will be returned for the type
      ///    requested.
      /// </param>
      /// <returns>System.Object.</returns>
      /// <summary>
      /// Resolves the specified type requested t.
      /// </summary>
      /// <param name="typeRequestedT">The type requested t.</param>
      /// <param name="ruleRequested">The rule requested.</param>
      /// <param name="boundParent">The bound parent.</param>
      /// <param name="conflictResolver">The conflict resolver.</param>
      /// <returns>System.Object.</returns>
      public object Resolve(Type         typeRequestedT,
                            StorageRules ruleRequested = StorageRules.AnyAccessLevel,
                            object       boundParent   = null,
                            Func<ConcurrentDictionary<Type, ITimeStampedCreatorAndStorageRules>, IConflictResolution>
                               conflictResolver =
                               null)
      {
         var qualifyingRegistrations = GetQualifyingRegistrations(typeRequestedT);

         if (qualifyingRegistrations.IsEmpty())
         {
            ThrowArgumentException(nameof(Resolve), "No registered contracts for the type " + typeRequestedT);
            return null;
         }

         // We have at least one qualifying registration.
         // Filter against storage rules.
         // If the qualifying registration is 'any', then any requested storage works.
         // If the requested storage rule is 'any', that also works.
         // Otherwise, the qualifying registration storage rule and the requested storage rule *must* match.
         if (ruleRequested != StorageRules.AnyAccessLevel)
         {
            // The rules qualify if they are "all" or if they match the requested rule.
            var storageMatchedQualifyingRegistrations =
               new ConcurrentDictionary<Type, ITimeStampedCreatorAndStorageRules>(
                                                                                  qualifyingRegistrations.Where(qr =>
                                                                                                                   qr
                                                                                                                     .Value
                                                                                                                     .CreatorsAndStorageRules
                                                                                                                         [typeRequestedT]
                                                                                                                     .ProvidedStorageRule ==
                                                                                                                   StorageRules
                                                                                                                     .AnyAccessLevel ||
                                                                                                                   qr
                                                                                                                     .Value
                                                                                                                     .CreatorsAndStorageRules
                                                                                                                         [typeRequestedT]
                                                                                                                     .ProvidedStorageRule ==
                                                                                                                   ruleRequested));
            if (storageMatchedQualifyingRegistrations.IsEmpty())
            {
               ThrowArgumentException(nameof(Resolve), "Cannot find a registration for the type ->" +
                                                       typeRequestedT                               +
                                                       "<- using storage rule ->"                   +
                                                       ruleRequested                                + "<-");
               return null;
            }

            // Return this value to the original collection.
            qualifyingRegistrations = storageMatchedQualifyingRegistrations;
         }

         if (!qualifyingRegistrations.Any())
         {
            ThrowArgumentException(nameof(Resolve),
                                   "Cannot find a registration for the type ->" + typeRequestedT + "<-");
            return null;
         }

         var resolutionToSeek = default(KeyValuePair<Type, IProvideCreatorAndStorageRule>);

         // Will need to determine the type we are creating (not the typeRequestedT that the user will cast this as)
         var qualifyingMasterType = default(Type);

         // If more than one registration, see if we can decide with the conflict resolver
         if (CannotResolveConflicts(conflictResolver, qualifyingRegistrations, ref qualifyingMasterType,
                                    ref resolutionToSeek))
         {
            ThrowOperationException(nameof(Resolve), "Cannot determine which one of " +
                                                     qualifyingRegistrations.Count    +
                                                     " registrations to use through the provided conflict resolver.");
            return null;
         }

         // If no resolutionToSeek yet (steps above failed), find it through another means.
         if (resolutionToSeek.IsAnEqualObjectTo(default(KeyValuePair<Type, IProvideCreatorAndStorageRule>)))
         {
            // Unconditional; applies to any count for qualifyingRegistrations
            if (CannotFindObviousChoice(typeRequestedT, qualifyingRegistrations, ref resolutionToSeek))
            {
               ThrowArgumentException(nameof(Resolve),
                                      "Unexpected error fetching a known contract for the type " + typeRequestedT);
               return null;
            }
         }

         // If unset, set now
         if (qualifyingMasterType.IsAnEqualObjectTo(default(Type)))
         {
            // Take the *first* qualifying registration made
            qualifyingMasterType = qualifyingRegistrations.OrderBy(qr => qr.Value.WhenAdded).First().Key;
         }

         // The same as having no qualifying registrations. Should never occur based on the checks above.
         if (resolutionToSeek.IsAnEqualObjectTo(default(KeyValuePair<Type, IProvideCreatorAndStorageRule>)))
         {
            ThrowArgumentException(nameof(Resolve), "Cannot find a registration for the type " + typeRequestedT);
            return null;
         }

         // Verify that the result will be legal, since we will cast as TypeRequestedT
         if (!qualifyingMasterType.IsTypeOrAssignableFromType(typeRequestedT))
         {
            ThrowOperationException(nameof(Resolve), "Cannot save an instance of ->" + qualifyingMasterType +
                                                     "<- as ->"                      + typeRequestedT       + "<-");
            return null;
         }

         // Now we have:
         // 1. A master type to create;
         // 2. A TypeRequestedT type to save as -- these might not be the same, but must relate class-design-wise
         // 3. A resolution to seek that might include an instance creator;
         // 4. If no instance creator, then we will use activator create instance

         // We switch here; the resolutionToSeek now has authority over our process.

         // This means that we also accept the resolutionToSeek's Type
         var finalTypeRequestedT = resolutionToSeek.Key;
         ruleRequested = resolutionToSeek.Value.ProvidedStorageRule;

         // Verify that we do not have an incompatible rules request and bound object
         if (ruleRequested == StorageRules.SharedDependencyBetweenInstances && boundParent == null)
         {
            ThrowArgumentException(nameof(Resolve),
                                   "Cannot resolve a shared type without a bound instance to attach it to. To share a variable without a bound instance, use a global singleton.");
            return null;
         }

         if (ruleRequested == StorageRules.GlobalSingleton)
         {
            if (_globalSingletonsByType.ContainsKey(finalTypeRequestedT))
            {
               var foundGlobalInstance = _globalSingletonsByType[finalTypeRequestedT];

               // If the instance is invalid -- which should not generally occur -- remove it.
               if (foundGlobalInstance == null)
               {
                  _globalSingletonsByType.Remove(finalTypeRequestedT);
               }
               // ELSE return it
               else
               {
                  return foundGlobalInstance;
               }
            }

            // ELSE proceed to creating the instance
         }
         else if (ruleRequested == StorageRules.SharedDependencyBetweenInstances)
         {
            // There is a remote possibility that after being stored as a shared instance, they key will get garbage
            //    collected, making it null. This case is ignored because we cannot get its type, etc.
            // NOTE that we ask for the qualifyingMasterType.  That is what we add down below (see line 842,
            //    "_sharedInstancesWithBoundMembers.Add(sharedInstance, new List<object>(boundParents));" --
            //    the key, sharedInstance, is of type qualifyingMasterType.
            var foundSharedInstances =
               _sharedInstancesWithBoundMembers.FirstOrDefault(si =>
                                                                  si.Key != null && si.Key.GetType() ==
                                                                  qualifyingMasterType);

            // If valid (not empty)
            if (foundSharedInstances.IsNotAnEqualObjectTo(default(KeyValuePair<object, List<object>>)))
            {
               // Add our bound object to the list... maybe make sure it's not there already..
               // Also ensure that we are not trying to bind the foundSharedInstances.kKey --
               //    that is the instance being returned, so cannot act as its own parent.

               // This is a critical error, so wil throw rather than return a value.
               var illegalBoundParent = ReferenceEquals(boundParent, foundSharedInstances.Key);

               if (illegalBoundParent)
               {
                  ThrowArgumentException(nameof(Resolve), "A returned instance cannot be bound to itself.");
                  return null;
               }

               // If the bound parent is already there, do not ree-add it.
               var parentIsAlreadyStored = foundSharedInstances.Value.Any(existingBoundParent =>
                                                                             ReferenceEquals(existingBoundParent,
                                                                                             boundParent));
               if (!parentIsAlreadyStored)
               {
                  _sharedInstancesWithBoundMembers[foundSharedInstances.Key].Add(boundParent);
               }

               // Return the key object, as that is the one that is being shared between the list of bound instances
               return foundSharedInstances.Key;
            }
         }

         // ELSE must instantiate

         // Try the creator
         object instantiatedObject = null;

         if (ProvidedCreatorFailed(resolutionToSeek, finalTypeRequestedT, ref instantiatedObject))
         {
            ThrowArgumentException(nameof(Resolve),
                                   "Provided constructor did not create the expected type ->" + finalTypeRequestedT +
                                   "<-");
            return null;
         }

         if (CouldNotCreateObject(qualifyingMasterType, ref instantiatedObject))
         {
            ThrowOperationException(nameof(Resolve),
                                    "Could not construct an object for base type ->" + qualifyingMasterType + "<-");
            return null;
         }

         // The last step is to determine if we have to save the new object in our container.

         // If global, yes -- this will only occur once
         switch (ruleRequested)
         {
            case StorageRules.GlobalSingleton:
               CreateSingletonInstance(instantiatedObject, finalTypeRequestedT);
               break;

            case StorageRules.SharedDependencyBetweenInstances:
               CreateSharedInstances(instantiatedObject, finalTypeRequestedT, boundParent);
               break;

            //default:
            //   //case StorageRules.AnyAccessLevel:
            //   //case StorageRules.DoNotStore:
            //   // DO NOTHING -- the instance manages itself locally
            //   break;
         }

         return instantiatedObject;
      }

      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      // R E G I S T E R   T Y P E   C O N T R A C T S
      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      // R E S O L V E
      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      /// <summary>
      ///    Removes a list of types that the parent type can be resolved as. Includes creators and storage rules.
      /// </summary>
      /// <typeparam name="TParent">The generic parent type</typeparam>
      /// <param name="typesToUnregister">The types to remove.</param>
      /// <summary>
      /// Removes a list of types that the parent type can be resolved as. Includes creators and storage rules.
      /// </summary>
      /// <typeparam name="TParent">The generic parent type</typeparam>
      /// <param name="typesToUnregister">The types to remove.</param>
      public void UnregisterTypeContracts<TParent>(params Type[] typesToUnregister)
      {
         if (!_registeredTypeContracts.ContainsKey(typeof(TParent)))
         {
            ThrowArgumentException(nameof(UnregisterTypeContracts),
                                   "The type ->" + typeof(TParent) +
                                   "<- cannot be unregistered because it was never is not registered");
            return;
         }

         // If no sub types named, try to clear the main type, which kills all sub elements
         if (typesToUnregister.IsEmpty())
         {
            _registeredTypeContracts.Remove(typeof(TParent));

            return;
         }

         // ELSE remove the sub types individually
         var parentRegistrations = _registeredTypeContracts[typeof(TParent)];

         foreach (var type in typesToUnregister)
         {
            if (parentRegistrations.CreatorsAndStorageRules.ContainsKey(type))
            {
               parentRegistrations.CreatorsAndStorageRules.Remove(type);
            }
         }

         // If this leaves the collection empty, remove it.
         if (parentRegistrations.CreatorsAndStorageRules.IsEmpty())
         {
            _registeredTypeContracts.Remove(typeof(TParent));
         }
      }

      #endregion Public Methods

      #region Protected Methods

      /// <summary>
      /// Clears the exceptions.
      /// </summary>
      protected void ClearExceptions()
      {
         IsArgumentExceptionThrown  = string.Empty;
         IsOperationExceptionThrown = string.Empty;
      }

      /// <summary>
      /// Adds a list of bound instances to a single shared instance.
      /// </summary>
      /// <param name="sharedInstance">The shared instance.</param>
      /// <param name="sharedInstanceType">The type of the object. The shared instance is often just an object without a final
      /// type.</param>
      /// <param name="boundParents">The bound member. Each of these are a different "parent" to the same shared instance.</param>
      protected void CreateSharedInstances(object          sharedInstance,
                                           Type            sharedInstanceType,
                                           params object[] boundParents)
      {
         if (!_sharedInstancesWithBoundMembers.ContainsKey(sharedInstance))
         {
            _sharedInstancesWithBoundMembers.Add(sharedInstance, new List<object>(boundParents));
         }
         else
         {
            var boundInstances = _sharedInstancesWithBoundMembers[sharedInstance];

            foreach (var parent in boundParents)
            {
               if (boundInstances.All(bi => !ReferenceEquals(bi, parent)))
               {
                  boundInstances.Add(parent);
               }
            }
         }
      }

      /// <summary>
      /// Adds a key-value pair with a class type and an instance of that class.
      /// </summary>
      /// <param name="instance">The instance of the type.</param>
      /// <param name="typeToSaveAs">The keyed type for storage.</param>
      protected void CreateSingletonInstance(object instance,
                                             Type   typeToSaveAs)
      {
         if (!_globalSingletonsByType.ContainsKey(typeToSaveAs))
         {
            _globalSingletonsByType.Add(typeToSaveAs, instance);
         }
         else
         {
            _globalSingletonsByType[typeToSaveAs] = instance;
         }
      }

      /// <summary>
      /// Releases unmanaged and - optionally - managed resources.
      /// </summary>
      /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
      /// unmanaged resources.</param>
      protected virtual void Dispose(bool disposing)
      {
         ReleaseUnmanagedResources();
         if (disposing)
         { }
      }

      /// <summary>
      /// Releases the unmanaged resources.
      /// </summary>
      protected virtual void ReleaseUnmanagedResources()
      {
         ResetContainer();
      }

      /// <summary>
      /// Removes a bound instance from all shared instances. Also cleans up any orphaned shared instances.
      /// </summary>
      /// <param name="obj">The object.</param>
      protected void RemoveBoundSharedDependencies(object obj)
      {
         if (_sharedInstancesWithBoundMembers.IsEmpty())
         {
            return;
         }

         var sharedInstancesIndexesToDelete = new List<int>();

         for (var sharedInstanceIdx = 0;
              sharedInstanceIdx < _sharedInstancesWithBoundMembers.Count;
              sharedInstanceIdx++)
         {
            var keyToSeek         = _sharedInstancesWithBoundMembers.Keys.ToList()[sharedInstanceIdx];
            var sharedInstance    = _sharedInstancesWithBoundMembers[keyToSeek];
            var foundBoundMembers = sharedInstance.Where(si => ReferenceEquals(si, obj)).ToArray();

            if (!foundBoundMembers.Any())
            {
               continue;
            }

            foreach (var foundBoundMember in foundBoundMembers)
            {
               sharedInstance.Remove(foundBoundMember);
            }

            // If the shared instance is empty, it must be removed, but best to do so after this iteration.
            if (sharedInstance.IsEmpty())
            {
               sharedInstancesIndexesToDelete.Add(sharedInstanceIdx);
            }
         }

         // Remove the shared instances that are now orphaned
         foreach (var sharedInstanceIdx in sharedInstancesIndexesToDelete.ToArray())
         {
            // This has been revised, so will fail on removal
            var keyToSeek = _sharedInstancesWithBoundMembers.Keys.ToList()[sharedInstanceIdx];

            _sharedInstancesWithBoundMembers.Remove(keyToSeek);
         }
      }

      /// <summary>
      /// If the type exists as a shared instance, removes it. Only used if this shared instance is about to go out of view.
      /// </summary>
      /// <typeparam name="ObjectT">The type of the object t.</typeparam>
      /// <param name="obj">The object.</param>
      protected void RemoveSharedInstance<ObjectT>(ObjectT obj)
      {
         // Seek these by reference, since we have a valid object. It's safer considering that any instantiated type can
         // be returned as any implemented interface. So the type could easily mis-match.
         var foundSharedInstance =
            _sharedInstancesWithBoundMembers.FirstOrDefault(si => ReferenceEquals(si.Key, obj));

         if (foundSharedInstance.IsNotAnEqualObjectTo(default(KeyValuePair<object, List<object>>)))
         {
            _sharedInstancesWithBoundMembers.Remove(foundSharedInstance.Key);
         }
      }

      /// <summary>
      /// Removes a global instance as long as it is the same type and reference.
      /// </summary>
      /// <typeparam name="ObjectT">The type of the object t.</typeparam>
      /// <param name="obj">The object.</param>
      protected void RemoveSingletonInstance<ObjectT>(ObjectT obj)
      {
         // Find the singleton based on the reference and *not* by the type, as any interface type might be declared, but
         // the constructor could easily hand us a base type.
         var foundSingleton = _globalSingletonsByType.FirstOrDefault(s => ReferenceEquals(s.Value, obj));

         if (foundSingleton.IsNotAnEqualObjectTo(default(KeyValuePair<Type, object>)))
         {
            _globalSingletonsByType.Remove(foundSingleton.Key);
         }
      }

      /// <summary>
      /// Clears all internal lists and exceptions.
      /// </summary>
      protected void ResetContainer()
      {
         _globalSingletonsByType?.Clear();
         _registeredTypeContracts?.Clear();
         _sharedInstancesWithBoundMembers?.Clear();

         ClearExceptions();
      }

      #endregion Protected Methods

      #region Private Methods

      /// <summary>
      /// Creates the complete message.
      /// </summary>
      /// <param name="methodName">Name of the method.</param>
      /// <param name="message">The message.</param>
      /// <returns>System.String.</returns>
      private static string CreateCompleteMessage(string methodName,
                                                  string message)
      {
         var finalMessage = nameof(SmartDIContainer).ToUpper() + ": " + methodName.ToUpper() + ": " + message;
         return finalMessage;
      }

      /// <summary>
      /// Looks for a valid registration as the "best choice" for the current conditions.
      /// </summary>
      /// <param name="typeRequestedT">The type requested t.</param>
      /// <param name="registrations">The registrations.</param>
      /// <param name="resolutionToSeek">The resolution to seek.</param>
      /// <returns><c>true</c> if a valid registration is found, else <c>false</c>.</returns>
      private bool CannotFindObviousChoice(
         Type                                                           typeRequestedT,
         ConcurrentDictionary<Type, ITimeStampedCreatorAndStorageRules> registrations,
         ref KeyValuePair<Type, IProvideCreatorAndStorageRule>          resolutionToSeek)
      {
         // Sort by date/time added, then select
         var foundDict = registrations.OrderBy(qr => qr.Value.WhenAdded).First().Value;
         var found     = false;

         foreach (var keyValuePair in foundDict.CreatorsAndStorageRules)
         {
            if (keyValuePair.Key == typeRequestedT)
            {
               resolutionToSeek = keyValuePair;
               found            = true;
               break;
            }
         }

         // Should never fail based on previous checks -- we ensured that at least one entry existed
         return !found;
      }

      /// <summary>
      /// Attempts to resolve conlicts, if they exist.
      /// </summary>
      /// <param name="conflictResolver">The conflict resolver.</param>
      /// <param name="qualifyingRegistrations">The qualifying registrations.</param>
      /// <param name="qualifyingMasterType">Type of the qualifying master.</param>
      /// <param name="resolutionToSeek">The resolution to seek.</param>
      /// <returns><c>true</c> if all conflicts have been resolved, else <c>false</c>.</returns>
      private bool CannotResolveConflicts(
         Func<ConcurrentDictionary<Type, ITimeStampedCreatorAndStorageRules>, IConflictResolution> conflictResolver,
         ConcurrentDictionary<Type, ITimeStampedCreatorAndStorageRules>
            qualifyingRegistrations,
         ref Type                                              qualifyingMasterType,
         ref KeyValuePair<Type, IProvideCreatorAndStorageRule> resolutionToSeek)
      {
         if (qualifyingRegistrations.Count > 1 && conflictResolver != null)
         {
            var masterResolution = conflictResolver(qualifyingRegistrations);

            if (masterResolution != null)
            {
               qualifyingMasterType = masterResolution.MasterType;
               resolutionToSeek     = masterResolution.TypeToCastWithStorageRule;
            }

            if (masterResolution == null
              ||
                qualifyingMasterType.IsNotAnEqualObjectTo(default(Type))
              ||
                resolutionToSeek.IsAnEqualObjectTo(default(KeyValuePair<Type, IProvideCreatorAndStorageRule>))
            )
            {
               return true;
            }
         }

         return false;
      }

      /// <summary>
      /// Tries to create an object and reports the result.
      /// </summary>
      /// <param name="qualifyingMasterType">Type of the qualifying master.</param>
      /// <param name="instantiatedObject">The instantiated object.</param>
      /// <returns><c>true</c> if object was created successfully, else <c>false</c>.</returns>
      private bool CouldNotCreateObject(Type       qualifyingMasterType,
                                        ref object instantiatedObject)
      {
         if (instantiatedObject == null)
         {
            // No choice but to use activator create instance Note that we actually *create* the qualifyingMasterType and
            // then *save as* the finalTypeRequestedT

            // Get the constructors and order by the fewest parameters -- HACK -- not very smart
            var availableConstructors =
               // ReSharper disable once UseCollectionCountProperty -- will not compile as the property "Count"
               qualifyingMasterType?.GetConstructors().OrderBy(c => c.GetParameters().Count()).ToArray();

            if (availableConstructors.IsEmpty())
            {
               ThrowArgumentException(nameof(Resolve),
                                      "Could not find a valid constructor for type requested ->" +
                                      qualifyingMasterType                                       + "<-");
               return true;
            }

            // try each constructor one at a time until we succeed
            // ReSharper disable once PossibleNullReferenceException -
            //     physically impossible based on "the previous statement" two lines before
            foreach (var constructor in availableConstructors)
            {
               var constructorParameters = constructor.GetParameters();

               if (constructorParameters.IsEmpty())
               {
                  instantiatedObject = Activator.CreateInstance(qualifyingMasterType);
                  break;
               }

               var parameters  = new List<object>();
               var skipThisOne = false;

               foreach (var parameterInfo in constructorParameters)
               {
                  object variableToInjectAsParameter = null;

                  try
                  {
                     variableToInjectAsParameter = Resolve(parameterInfo.ParameterType);
                     skipThisOne                 = variableToInjectAsParameter == null;
                  }
                  catch (Exception)
                  {
                     // If we threw, then this constructor will not work
                     Debug.WriteLine("SmartDIContainer: Resolve: Exception on attempt to instantiate " +
                                     qualifyingMasterType                                              +
                                     " using one of its constructors. Will try another constructor...");
                     skipThisOne = true;
                  }

                  if (skipThisOne)
                  {
                     break;
                  }

                  parameters.Add(variableToInjectAsParameter);
               }

               if (skipThisOne)
               {
                  continue;
               }

               instantiatedObject = constructor.Invoke(parameters.ToArray());
            }
         }

         if (instantiatedObject == null)
         {
            return true;
         }

         return false;
      }

      /// <summary>
      /// Gets the qualifying registrations.
      /// </summary>
      /// <param name="typeRequestedT">The type requested t.</param>
      /// <returns>ConcurrentDictionary&lt;Type, ITimeStampedCreatorAndStorageRules&gt;.</returns>
      private ConcurrentDictionary<Type, ITimeStampedCreatorAndStorageRules> GetQualifyingRegistrations(
         Type typeRequestedT)
      {
         // Create a dictionary for the sub-selection of just contracts that resolve typeRequestedT
         var qualifyingRegistrations = new ConcurrentDictionary<Type, ITimeStampedCreatorAndStorageRules>();

         // Also store which of these is an "any" access contract
         var qualifyingAnyAccessRegistrations = new ConcurrentDictionary<Type, ITimeStampedCreatorAndStorageRules>();

         foreach (var contract in
            _registeredTypeContracts.Where(rc => rc.Value.CreatorsAndStorageRules.ContainsKey(typeRequestedT)))
         {
            qualifyingRegistrations.AddOrUpdate(contract.Key,
                                                new TimeStampedCreatorAndStorageRules
                                                {
                                                   WhenAdded               = contract.Value.WhenAdded,
                                                   CreatorsAndStorageRules = contract.Value.CreatorsAndStorageRules
                                                });

            if (contract.Value.CreatorsAndStorageRules[typeRequestedT].ProvidedStorageRule ==
                StorageRules.AnyAccessLevel)
            {
               qualifyingAnyAccessRegistrations.AddOrUpdate(contract.Key, contract.Value);
            }
         }

         if (qualifyingRegistrations.Count > 1)
         {
            if (ThrowOnMultipleRegisteredTypesForOneResolvedType)
            {
               ThrowArgumentException(nameof(Resolve),
                                      "Too many contracts ("         + qualifyingRegistrations.Count +
                                      ") for the resolvable type ->" + typeRequestedT                + "<-");
               return qualifyingRegistrations;
            }
         }

         return qualifyingRegistrations;
      }

      /// <summary>
      /// Attempts to use the provided instance creator, if available, and reports the result.
      /// </summary>
      /// <param name="resolutionToSeek">The resolution to seek.</param>
      /// <param name="finalTypeRequestedT">The final type requested t.</param>
      /// <param name="instantiatedObject">The instantiated object.</param>
      /// <returns><c>true</c> if a provided creator exists but failed to instantiate an object, else <c>false</c>.</returns>
      private bool ProvidedCreatorFailed(KeyValuePair<Type, IProvideCreatorAndStorageRule> resolutionToSeek,
                                         Type                                              finalTypeRequestedT,
                                         ref object                                        instantiatedObject)
      {
         if (resolutionToSeek.Value.ProvidedCreator != null)
         {
            instantiatedObject = resolutionToSeek.Value.ProvidedCreator();

            if (instantiatedObject == null)
            {
               ThrowArgumentException(nameof(Resolve),
                                      "Could not create an object using the provided constructor.");
               return true;
            }

            // if (!instantiatedObject.GetType().IsTypeOrAssignableFromType(finalTypeRequestedT))
            if (!finalTypeRequestedT.IsTypeOrAssignableFromType(instantiatedObject.GetType()))
            {
               return true;
            }

            // ELSE the instated object is valid. Proceed as if we had created the object using activator create instance.
         }

         return false;
      }

      /// <summary>
      /// Seeks the existing contract.
      /// </summary>
      /// <param name="classT">The class t.</param>
      /// <param name="creatorsAndRules">The creators and rules.</param>
      /// <returns>ITimeStampedCreatorAndStorageRules.</returns>
      private ITimeStampedCreatorAndStorageRules SeekExistingContract(Type classT,
                                                                      IDictionary<Type, IProvideCreatorAndStorageRule>
                                                                         creatorsAndRules)
      {
         var existingContract = _registeredTypeContracts[classT];

         // Augment whatever is there, one at a time.
         foreach (var keyValuePair in creatorsAndRules)
         {
            // The existingContract is a dictionary of types and storage rules in matching pairs. This means that we
            // *cannot* add a creator and rule that is for the same resolved type but a different storage rule.
            var existingTypeContract =
               existingContract.CreatorsAndStorageRules.FirstOrDefault(ec => ec.Key == keyValuePair.Key);

            if (existingTypeContract.IsNotAnEqualObjectTo(default(KeyValuePair<Type, IProvideCreatorAndStorageRule>))
            )
            {
               // We have the same type key, so can only over-write... or throw
               if (ThrowOnAttemptToAssignDuplicateContractSubType)
               {
                  ThrowArgumentException(nameof(RegisterTypeContracts),
                                         "Cannot replace an existing type and storage rule contract.  Pleased remove the old one first.  Type ->" +
                                         existingTypeContract
                                           .Key                                         +
                                         "<- storage rule ->"                           +
                                         existingTypeContract.Value.ProvidedStorageRule +
                                         "<-");
               }

               // ELSE
               // Falls through; nothing is illegal
            }
            else
            {
               // Does not exist yet, so add it.
               existingContract.CreatorsAndStorageRules.Add(keyValuePair);
            }
         }

         return existingContract;
      }

      /// <summary>
      /// Throws the argument exception.
      /// </summary>
      /// <param name="methodName">Name of the method.</param>
      /// <param name="message">The message.</param>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="System.ArgumentException"></exception>
      private void ThrowArgumentException(string methodName,
                                          string message)
      {
         var finalMessage = CreateCompleteMessage(methodName, message);

         if (IsUnitTesting)
         {
            IsArgumentExceptionThrown = finalMessage;
         }
         else
         {
            throw new ArgumentException(finalMessage);
         }
      }

      /// <summary>
      /// Throws the operation exception.
      /// </summary>
      /// <param name="methodName">Name of the method.</param>
      /// <param name="message">The message.</param>
      /// <exception cref="InvalidOperationException"></exception>
      /// <exception cref="System.InvalidOperationException"></exception>
      private void ThrowOperationException(string methodName,
                                           string message)
      {
         var finalMessage = CreateCompleteMessage(methodName, message);

         if (IsUnitTesting)
         {
            IsOperationExceptionThrown = finalMessage;
         }
         else
         {
            throw new InvalidOperationException(finalMessage);
         }
      }

      #endregion Private Methods
   }
}