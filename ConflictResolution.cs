#region License

// Copyright (c) 2019  Marcus Technical Services, Inc. <marcus@marcusts.com>
//
// This file, ConflictResolution.cs, is a part of a program called AccountViewMobile.
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
   /// Interface IConflictResolution
   /// </summary>
   public interface IConflictResolution
   {
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
   }
}