<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="Hookmeupcloud" generation="1" functional="0" release="0" Id="986340fe-b297-4eec-a6df-b4c132319496" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="HookmeupcloudGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="hookmeupnew:HttpIn" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/Hookmeupcloud/HookmeupcloudGroup/LB:hookmeupnew:HttpIn" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="hookmeupnew:LogLevel" defaultValue="">
          <maps>
            <mapMoniker name="/Hookmeupcloud/HookmeupcloudGroup/Maphookmeupnew:LogLevel" />
          </maps>
        </aCS>
        <aCS name="hookmeupnew:OfflineSchedulerInterval" defaultValue="">
          <maps>
            <mapMoniker name="/Hookmeupcloud/HookmeupcloudGroup/Maphookmeupnew:OfflineSchedulerInterval" />
          </maps>
        </aCS>
        <aCS name="hookmeupnew:PerfCounterSampleRate" defaultValue="">
          <maps>
            <mapMoniker name="/Hookmeupcloud/HookmeupcloudGroup/Maphookmeupnew:PerfCounterSampleRate" />
          </maps>
        </aCS>
        <aCS name="hookmeupnew:PerfCounterSpecifier" defaultValue="">
          <maps>
            <mapMoniker name="/Hookmeupcloud/HookmeupcloudGroup/Maphookmeupnew:PerfCounterSpecifier" />
          </maps>
        </aCS>
        <aCS name="hookmeupnew:RemoteDataStorage" defaultValue="">
          <maps>
            <mapMoniker name="/Hookmeupcloud/HookmeupcloudGroup/Maphookmeupnew:RemoteDataStorage" />
          </maps>
        </aCS>
        <aCS name="hookmeupnewInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Hookmeupcloud/HookmeupcloudGroup/MaphookmeupnewInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:hookmeupnew:HttpIn">
          <toPorts>
            <inPortMoniker name="/Hookmeupcloud/HookmeupcloudGroup/hookmeupnew/HttpIn" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="Maphookmeupnew:LogLevel" kind="Identity">
          <setting>
            <aCSMoniker name="/Hookmeupcloud/HookmeupcloudGroup/hookmeupnew/LogLevel" />
          </setting>
        </map>
        <map name="Maphookmeupnew:OfflineSchedulerInterval" kind="Identity">
          <setting>
            <aCSMoniker name="/Hookmeupcloud/HookmeupcloudGroup/hookmeupnew/OfflineSchedulerInterval" />
          </setting>
        </map>
        <map name="Maphookmeupnew:PerfCounterSampleRate" kind="Identity">
          <setting>
            <aCSMoniker name="/Hookmeupcloud/HookmeupcloudGroup/hookmeupnew/PerfCounterSampleRate" />
          </setting>
        </map>
        <map name="Maphookmeupnew:PerfCounterSpecifier" kind="Identity">
          <setting>
            <aCSMoniker name="/Hookmeupcloud/HookmeupcloudGroup/hookmeupnew/PerfCounterSpecifier" />
          </setting>
        </map>
        <map name="Maphookmeupnew:RemoteDataStorage" kind="Identity">
          <setting>
            <aCSMoniker name="/Hookmeupcloud/HookmeupcloudGroup/hookmeupnew/RemoteDataStorage" />
          </setting>
        </map>
        <map name="MaphookmeupnewInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Hookmeupcloud/HookmeupcloudGroup/hookmeupnewInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="hookmeupnew" generation="1" functional="0" release="0" software="C:\Users\ma041sa\Documents\Visual Studio 2012\Projects\HookMeUp\Hookmeupcloud\csx\Release\roles\hookmeupnew" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="HttpIn" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="LogLevel" defaultValue="" />
              <aCS name="OfflineSchedulerInterval" defaultValue="" />
              <aCS name="PerfCounterSampleRate" defaultValue="" />
              <aCS name="PerfCounterSpecifier" defaultValue="" />
              <aCS name="RemoteDataStorage" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;hookmeupnew&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;hookmeupnew&quot;&gt;&lt;e name=&quot;HttpIn&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Hookmeupcloud/HookmeupcloudGroup/hookmeupnewInstances" />
            <sCSPolicyUpdateDomainMoniker name="/Hookmeupcloud/HookmeupcloudGroup/hookmeupnewUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/Hookmeupcloud/HookmeupcloudGroup/hookmeupnewFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="hookmeupnewUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="hookmeupnewFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="hookmeupnewInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="c3f4621f-aace-428c-a57e-51abcb9c0ed6" ref="Microsoft.RedDog.Contract\ServiceContract\HookmeupcloudContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="36663740-6790-4b8a-8334-71d1f65458a7" ref="Microsoft.RedDog.Contract\Interface\hookmeupnew:HttpIn@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/Hookmeupcloud/HookmeupcloudGroup/hookmeupnew:HttpIn" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>