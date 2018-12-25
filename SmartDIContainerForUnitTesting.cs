// *********************************************************************************
// Assembly         : Com.MarcusTS.SmartDI
// Author           : Stephen Marcus (Marcus Technical Services, Inc.)
// Created          : 05-07-2018
// Last Modified On : 12-24-2018
//
// <copyright file="SmartDIContainerForUnitTesting.cs" company="Marcus Technical Services, Inc.">
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
   using System.Collections.Generic;

   /// <summary>
   ///    Interface ISmartDIContainerForUnitTesting
   ///    Implements the <see cref="ISmartDIContainer" />
   ///    Implements the <see cref="Com.MarcusTS.SmartDI.ISmartDIContainer" />
   /// </summary>
   /// <seealso cref="Com.MarcusTS.SmartDI.ISmartDIContainer" />
   /// <seealso cref="ISmartDIContainer" />
   /// <summary>
   /// Interface ISmartDIContainerForUnitTesting
   /// Implements the <see cref="Com.MarcusTS.SmartDI.ISmartDIContainer" />
   /// </summary>
   /// <seealso cref="Com.MarcusTS.SmartDI.ISmartDIContainer" />
   public interface ISmartDIContainerForUnitTesting : ISmartDIContainer
   {
      #region Public Properties

      /// <summary>
      ///    Gets the exposed global singletons.
      /// </summary>
      /// <value>The exposed global singletons.</value>
      /// <summary>
      /// Gets the exposed global singletons.
      /// </summary>
      /// <value>The exposed global singletons.</value>
      IDictionary<Type, object> ExposedGlobalSingletons { get; }

      /// <summary>
      ///    Gets the exposed registered type contracts.
      /// </summary>
      /// <value>The exposed registered type contracts.</value>
      /// <summary>
      /// Gets the exposed registered type contracts.
      /// </summary>
      /// <value>The exposed registered type contracts.</value>
      IDictionary<Type, ITimeStampedCreatorAndStorageRules> ExposedRegisteredTypeContracts { get; }

      /// <summary>
      ///    Gets the exposed shared instances with bound members.
      /// </summary>
      /// <value>The exposed shared instances with bound members.</value>
      /// <summary>
      /// Gets the exposed shared instances with bound members.
      /// </summary>
      /// <value>The exposed shared instances with bound members.</value>
      IDictionary<object, IList<object>> ExposedSharedInstancesWithBoundMembers { get; }

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

      #endregion Public Properties

      #region Public Methods

      /// <summary>
      /// Clears the unit test exceptions.
      /// </summary>
      void ClearUnitTestExceptions();

      /// <summary>
      /// Resets the unit test container.
      /// </summary>
      void ResetUnitTestContainer();

      #endregion Public Methods
   }

   /// <summary>
   /// Class SafeDiContainerForUnitTesting.
   /// Implements the <see cref="SmartDIContainer" />
   /// Implements the <see cref="ISmartDIContainerForUnitTesting" />
   /// Implements the <see cref="Com.MarcusTS.SmartDI.SmartDIContainer" />
   /// Implements the <see cref="Com.MarcusTS.SmartDI.ISmartDIContainerForUnitTesting" />
   /// </summary>
   /// <seealso cref="Com.MarcusTS.SmartDI.SmartDIContainer" />
   /// <seealso cref="Com.MarcusTS.SmartDI.ISmartDIContainerForUnitTesting" />
   /// <seealso cref="SmartDIContainer" />
   /// <seealso cref="ISmartDIContainerForUnitTesting" />
   public class SafeDiContainerForUnitTesting : SmartDIContainer, ISmartDIContainerForUnitTesting
   {
      #region Public Constructors

      /// <summary>
      /// Initializes a new instance of the <see cref="SafeDiContainerForUnitTesting" /> class.
      /// </summary>
      public SafeDiContainerForUnitTesting()
      {
         IsUnitTesting = true;
      }

      #endregion Public Constructors

      #region Public Properties

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
      public IDictionary<object, IList<object>> ExposedSharedInstancesWithBoundMembers =>
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

      #endregion Public Properties

      #region Public Methods

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

      #endregion Public Methods
   }
}