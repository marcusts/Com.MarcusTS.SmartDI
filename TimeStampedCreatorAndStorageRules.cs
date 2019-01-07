// *********************************************************************************
// Assembly         : Com.MarcusTS.SmartDI
// Author           : Stephen Marcus (Marcus Technical Services, Inc.)
// Created          : 12-21-2018
// Last Modified On : 12-27-2018
//
// <copyright file="TimeStampedCreatorAndStorageRules.cs" company="Marcus Technical Services, Inc.">
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
   ///    Interface ITimeStampedCreatorAndStorageRules
   /// </summary>
   public interface ITimeStampedCreatorAndStorageRules
   {
      #region Public Properties

      /// <summary>
      ///    Gets or sets the creators and storage rules.
      /// </summary>
      /// <value>The creators and storage rules.</value>
      IDictionary<Type, IProvideCreatorAndStorageRule> CreatorsAndStorageRules { get; set; }

      /// <summary>
      ///    Gets or sets the when added.
      /// </summary>
      /// <value>The when added.</value>
      DateTime WhenAdded { get; set; }

      #endregion Public Properties
   }

   /// <summary>
   ///    Class TimeStampedCreatorAndStorageRules.
   ///    Implements the <see cref="ITimeStampedCreatorAndStorageRules" />
   ///    Implements the <see cref="Com.MarcusTS.SmartDI.ITimeStampedCreatorAndStorageRules" />
   /// </summary>
   /// <seealso cref="Com.MarcusTS.SmartDI.ITimeStampedCreatorAndStorageRules" />
   /// <seealso cref="ITimeStampedCreatorAndStorageRules" />
   public class TimeStampedCreatorAndStorageRules : ITimeStampedCreatorAndStorageRules
   {
      #region Public Properties

      /// <summary>
      ///    Gets or sets the creators and storage rules.
      /// </summary>
      /// <value>The creators and storage rules.</value>
      public IDictionary<Type, IProvideCreatorAndStorageRule> CreatorsAndStorageRules { get; set; }

      /// <summary>
      ///    Gets or sets the when added.
      /// </summary>
      /// <value>The when added.</value>
      public DateTime WhenAdded { get; set; }

      #endregion Public Properties
   }
}