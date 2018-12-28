// *********************************************************************************
// Assembly         : Com.MarcusTS.SmartDI
// Author           : Stephen Marcus (Marcus Technical Services, Inc.)
// Created          : 05-20-2018
// Last Modified On : 12-27-2018
//
// <copyright file="ConflictResolution.cs" company="Marcus Technical Services, Inc.">
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
   /// Interface IConflictResolution
   /// </summary>
   public interface IConflictResolution
   {
      #region Public Properties

      /// <summary>
      /// Gets or sets the type of the master.
      /// </summary>
      /// <value>The type of the master.</value>
      Type MasterType { get; set; }

      /// <summary>
      /// Gets or sets the type to cast with storage rule.
      /// </summary>
      /// <value>The type to cast with storage rule.</value>
      KeyValuePair<Type, IProvideCreatorAndStorageRule> TypeToCastWithStorageRule { get; set; }

      #endregion Public Properties
   }

   /// <summary>
   /// Class ConflictResolution.
   /// Implements the <see cref="IConflictResolution" />
   /// Implements the <see cref="Com.MarcusTS.SmartDI.IConflictResolution" />
   /// </summary>
   /// <seealso cref="Com.MarcusTS.SmartDI.IConflictResolution" />
   /// <seealso cref="IConflictResolution" />
   public class ConflictResolution : IConflictResolution
   {
      #region Public Properties

      /// <summary>
      /// Gets or sets the type of the master.
      /// </summary>
      /// <value>The type of the master.</value>
      public Type MasterType { get; set; }

      /// <summary>
      /// Gets or sets the type to cast with storage rule.
      /// </summary>
      /// <value>The type to cast with storage rule.</value>
      public KeyValuePair<Type, IProvideCreatorAndStorageRule> TypeToCastWithStorageRule { get; set; }

      #endregion Public Properties
   }
}