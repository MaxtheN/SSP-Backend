import info from './info';
import arbitrationcourt from './arbitrationcourt';
import managment from './managment';
import document from './document';
import report from './report/index';
import proposal from './proposal';
import hrm from './hrm';
import salary from './salary';
import memship from './memship';
import claim from './claim';
import corruption from './corruption';
import websettings from './websettings';
import srv from './srv';
import dualedu from './dualedu';
import appeal from './appeal';
import kpi from './kpi';

// Array of sections
export default [
   ...info,
   ...document,
   ...report,
   ...memship,
   ...claim,
   ...corruption,
   ...srv,
   ...dualedu,
   ...arbitrationcourt,
   ...proposal,
   ...salary,
   ...hrm,
   ...appeal,
   ...kpi,
   ...websettings,
   ...managment
];
