#region License

// Copyright (c) 2019  Marcus Technical Services, Inc. <marcus@marcusts.com>
//
// This file, TimeStampedCreatorAndStorageRules.cs, is a part of a program called AccountViewMobile.
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
   /// Interface ITimeStampedCreatorAndStorageRules
   /// </summary>
   public interface ITimeStampedCreatorAndStorageRules
   {
      /// <summary>
      /// Gets or sets the creators and storage rules.
      /// </summary>
      /// <value>The creators and storage rules.</value>
      IDictionary<Type, IProvideCreatorAndStorageRule> CreatorsAndStorageRules { get; set; }

      /// <summary>
      /// Gets or sets the when added.
      /// </summary>
      /// <value>The when added.</value>
      DateTime WhenAdded { get; set; }
   }

   /// <summary>
   /// Class TimeStampedCreatorAndStorageRules.
   /// Implements the <see cref="ITimeStampedCreatorAndStorageRules" />
   /// Implements the <see cref="Com.MarcusTS.SmartDI.ITimeStampedCreatorAndStorageRules" />
   /// </summary>
   /// <seealso cref="Com.MarcusTS.SmartDI.ITimeStampedCreatorAndStorageRules" />
   /// <seealso cref="ITimeStampedCreatorAndStorageRules" />
   public class TimeStampedCreatorAndStorageRules : ITimeStampedCreatorAndStorageRules
   {
      /// <summary>
      /// Gets or sets the creators and storage rules.
      /// </summary>
      /// <value>The creators and storage rules.</value>
      public IDictionary<Type, IProvideCreatorAndStorageRule> CreatorsAndStorageRules { get; set; }

      /// <summary>
      /// Gets or sets the when added.
      /// </summary>
      /// <value>The when added.</value>
      public DateTime WhenAdded { get; set; }
   }
}