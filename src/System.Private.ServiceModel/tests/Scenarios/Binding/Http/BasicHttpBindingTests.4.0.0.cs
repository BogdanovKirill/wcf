﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Infrastructure.Common;
using Xunit;

public static class Binding_Http_BasicHttpBindingTests
{
    [WcfFact]
    [OuterLoop]
    public static void DefaultSettings_Echo_RoundTrips_String()
    {
        ChannelFactory<IWcfService> factory = null;
        IWcfService serviceProxy = null;
        string testString = "Hello";
        Binding binding = null;

        try
        {
            // *** SETUP *** \\
            binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            factory = new ChannelFactory<IWcfService>(binding, new EndpointAddress(Endpoints.HttpBaseAddress_Basic));
            serviceProxy = factory.CreateChannel();

            // *** EXECUTE *** \\
            string result = serviceProxy.Echo(testString);

            // *** VALIDATE *** \\
            Assert.True(result == testString, String.Format("Error: expected response from service: '{0}' Actual was: '{1}'", testString, result));

            // *** CLEANUP *** \\
            factory.Close();
            ((ICommunicationObject)serviceProxy).Close();
        }
        finally
        {
            // *** ENSURE CLEANUP *** \\
            ScenarioTestHelpers.CloseCommunicationObjects((ICommunicationObject)serviceProxy, factory);
        }
    }

    [WcfFact]
    [OuterLoop]
    public static void HttpKeepAliveDisabled_Echo_RoundTrips_True()
    {
        ChannelFactory<IWcfService> factory = null;
        IWcfService serviceProxy = null;
        Binding binding = null;
        CustomBinding customBinding = null;

        try
        {
            // *** SETUP *** \\
            binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            customBinding = new CustomBinding(binding);
            var httpElement = customBinding.Elements.Find<HttpTransportBindingElement>();
            httpElement.KeepAliveEnabled = false;

            factory = new ChannelFactory<IWcfService>(customBinding, new EndpointAddress(Endpoints.HttpBaseAddress_Basic));
            serviceProxy = factory.CreateChannel();

            // *** EXECUTE *** \\
            bool result = serviceProxy.IsHttpKeepAliveDisabled();

            // *** VALIDATE *** \\
            Assert.True(result, "Error: expected response from service: 'true' Actual was: 'false'");

            // *** CLEANUP *** \\
            factory.Close();
            ((ICommunicationObject)serviceProxy).Close();
        }
        finally
        {
            // *** ENSURE CLEANUP *** \\
            ScenarioTestHelpers.CloseCommunicationObjects((ICommunicationObject)serviceProxy, factory);
        }
    }
}
