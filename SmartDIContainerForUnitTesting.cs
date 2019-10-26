#region License

// Copyright (c) 2019  Marcus Technical Services, Inc. <marcus@marcusts.com>
//
// This file, SmartDIContainerForUnitTesting.cs, is a part of a program called AccountViewMobile.
//
// AccountViewMobile is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Permission to use, copy, modify, and/or distribute this software
// for any purpose with or without fee is hereby granted, provided
// that the above copyright notice and this permission notice appear
// in all copies.
//
// AccountViewMobile is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For the complete GNU General Public License,
// see <http://www.gnu.org/licenses/>.

#endregion

namespace Com.MarcusTS.SmartDI
{
   using System;
   using System.Collections.Generic;

   /// <summary>
   /// Interface ISmartDIContainerForUnitTesting
   /// Implements the <see cref="ISmartDIContainer" />
   /// Implements the <see cref="Com.MarcusTS.SmartDI.ISmartDIContainer" />
   /// </summary>
   /// <seealso cref="Com.MarcusTS.SmartDI.ISmartDIContainer" />
   /// <seealso cref="ISmartDIContainer" />
   /// <seealso cref="Com.MarcusTS.SmartDI.ISmartDIContainer" />
   public interface ISmartDIContainerForUnitTesting : ISmartDIContainer
   {
      /// <summary>
      /// Gets the exposed global singletons.
      /// </summary>
      /// <value>The exposed global singletons.</value>
      IDictionary<Type, object> ExposedGlobalSingletons { get; }

      /// <summary>
      /// Gets the exposed registered type contracts.
      /// </summary>
      /// <value>The exposed registered type contracts.</value>
      IDictionary<Type, ITimeStampedCreatorAndStorageRules> ExposedRegisteredTypeContracts { get; }

      /// <summary>
      /// Gets the exposed shared instances with bound members.
      /// </summary>
      /// <value>The exposed shared instances with bound members.</value>
      IDictionary<object, List<object>> ExposedSharedInstancesWithBoundMembers { get; }

      /// <summary>
      /// Gets or sets a value indicating whether [exposed throw on attempt to assign duplicate contract sub type].
      /// </summary>
      /// <value><c>true</c> if [exposed throw on attempt to assign duplicate contract sub type]; otherwise, <c>false</c>.</value>
      bool ExposedThrowOnAttemptToAssignDuplicateContractSubType { get; set; }

      /// <summary>
      /// Gets or sets a value indicating whether [exposed throw on multiple registered types for one resolved type].
      /// </summary>
      /// <value><c>true</c> if [exposed throw on multiple registered types for one resolved type]; otherwise, <c>false</c>.</value>
      bool ExposedThrowOnMultipleRegisteredTypesForOneResolvedType { get; set; }

      /// <summary>
      /// Gets the is argument exception.
      /// </summary>
      /// <value>The is argument exception.</value>
      string IsArgumentException { get; }

      /// <summary>
      /// Gets the is operation exception.
      /// </summary>
      /// <value>The is operation exception.</value>
      string IsOperationException { get; }

      /// <summary>
      /// Clears the unit test exceptions.
      /// </summary>
      void ClearUnitTestExceptions();

      /// <summary>
      /// Resets the unit test container.
      /// </summary>
      void ResetUnitTestContainer();
   }

   /// <summary>
   /// Class SmartDIContainerForUnitTesting.
   /// Implements the <see cref="SmartDIContainer" />
   /// Implements the <see cref="ISmartDIContainerForUnitTesting" />
   /// Implements the <see cref="Com.MarcusTS.SmartDI.SmartDIContainer" />
   /// Implements the <see cref="Com.MarcusTS.SmartDI.ISmartDIContainerForUnitTesting" />
   /// </summary>
   /// <seealso cref="Com.MarcusTS.SmartDI.SmartDIContainer" />
   /// <seealso cref="Com.MarcusTS.SmartDI.ISmartDIContainerForUnitTesting" />
   /// <seealso cref="SmartDIContainer" />
   /// <seealso cref="ISmartDIContainerForUnitTesting" />
   public class SmartDIContainerForUnitTesting : SmartDIContainer, ISmartDIContainerForUnitTesting
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="SmartDIContainerForUnitTesting" /> class.
      /// </summary>
      public SmartDIContainerForUnitTesting()
      {
         IsUnitTesting = true;
      }

      /// <summary>
      /// Gets the exposed global singletons.
      /// </summary>
      /// <value>The exposed global singletons.</value>
      public IDictionary<Type, object> ExposedGlobalSingletons => _globalSingletonsByType;

      /// <summary>
      /// Gets the exposed registered type contracts.
      /// </summary>
      /// <value>The exposed registered type contracts.</value>
      public IDictionary<Type, ITimeStampedCreatorAndStorageRules> ExposedRegisteredTypeContracts =>
         _registeredTypeContracts;

      /// <summary>
      /// Gets the exposed shared instances with bound members.
      /// </summary>
      /// <value>The exposed shared instances with bound members.</value>
      public IDictionary<object, List<object>> ExposedSharedInstancesWithBoundMembers =>
         _sharedInstancesWithBoundMembers;

      /// <summary>
      /// Gets or sets a value indicating whether [exposed throw on attempt to assign duplicate contract sub type].
      /// </summary>
      /// <value><c>true</c> if [exposed throw on attempt to assign duplicate contract sub type]; otherwise, <c>false</c>.</value>
      public bool ExposedThrowOnAttemptToAssignDuplicateContractSubType
      {
         get => ThrowOnAttemptToAssignDuplicateContractSubType;
         set => ThrowOnAttemptToAssignDuplicateContractSubType = value;
      }

      /// <summary>
      /// Gets or sets a value indicating whether [exposed throw on multiple registered types for one resolved type].
      /// </summary>
      /// <value><c>true</c> if [exposed throw on multiple registered types for one resolved type]; otherwise, <c>false</c>.</value>
      public bool ExposedThrowOnMultipleRegisteredTypesForOneResolvedType
      {
         get => ThrowOnMultipleRegisteredTypesForOneResolvedType;
         set => ThrowOnMultipleRegisteredTypesForOneResolvedType = value;
      }

      /// <summary>
      /// Gets the is argument exception.
      /// </summary>
      /// <value>The is argument exception.</value>
      public string IsArgumentException => IsArgumentExceptionThrown;

      /// <summary>
      /// Gets the is operation exception.
      /// </summary>
      /// <value>The is operation exception.</value>
      public string IsOperationException => IsOperationExceptionThrown;

      /// <summary>
      /// Clears the unit test exceptions.
      /// </summary>
      public void ClearUnitTestExceptions()
      {
         ClearExceptions();
      }

      /// <summary>
      /// Resets the unit test container.
      /// </summary>
      public void ResetUnitTestContainer()
      {
         ResetContainer();

         ThrowOnMultipleRegisteredTypesForOneResolvedType = false;
         ThrowOnAttemptToAssignDuplicateContractSubType   = false;
      }
   }
}