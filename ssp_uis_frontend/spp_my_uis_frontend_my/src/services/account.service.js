import ApiService from "./api.service";

const AccountService = {
  GetParentFromGovData(identityDocumentId, Seria, Number, DateOfBirth, pinfl) {
    return ApiService.get(
      `/Account/GetParentFromGovData?identityDocumentId=${identityDocumentId}&Seria=${Seria}&Number=${Number}&DateOfBirth=${DateOfBirth}&pinfl=${pinfl}`
    );
  },
  LoginByEImzo(data) {
    return ApiService.post("Account/LoginByEImzo", data);
  },
  GetChallenge() {
    return ApiService.get("account/GetChallenge");
  },
  OneIdLogin(data) {
    return ApiService.post("account/OneIdLogin", data);
  },
  GetFromSoliq(inn) {
    return ApiService.get(`/Account/GetFromSoliq/${inn}`);
  },
  SearchByInnPnfl(innpnfl) {
    return ApiService.get(`/Account/SearchByInnPnfl?innpnfl=${innpnfl}`);
  },
  GetFromSoliqByPinfl(pinfl) {
    return ApiService.get(`/Account/GetFromSoliqByPinfl/${pinfl}`);
  },
  InsertRegistration(data) {
    return ApiService.post("/Account/InsertRegistration", data);
  },
  SendSMSCode(data) {
    return ApiService.post("/Account/SendSMSCode", data);
  },
  GetChildrenFromGovData(DocumentSeries, DocumentNumber, DateOfBirth) {
    return ApiService.get(
      `/Account/GetChildrenFromGovData?DocumentSeries=${DocumentSeries}&DocumentNumber=${DocumentNumber}&DateOfBirth=${DateOfBirth}`
    );
  },
  GetContractorDocumentInfo() {
    return ApiService.get("/Account/GetContractorDocumentInfo");
  },
  GetContractorInfo() {
    return ApiService.get("/Account/GetContractorInfo");
  },
  SignIn(data) {
    return ApiService.post("/Account/SignIn", data);
  },
  SignInTwoFactor(data) {
    return ApiService.post("/Account/SignInTwoFactor", data);
  },
  IsCheckAccount(data) {
    return ApiService.post(`/Account/IsUserRegistered`, data);
  },
  GetChildrenFromERP(DocumentSeries, DocumentNumber, DateOfBirth) {
    return ApiService.get(
      `/Account/GetChildrenFromERP?DocumentSeries=${DocumentSeries}&DocumentNumber=${DocumentNumber}&DateOfBirth=${DateOfBirth}`
    );
  },
  GetChildrenData(
    admissiontypeid,
    identityDocumentId,
    documentSeries,
    documentNumber,
    dateOfBirth,
    lang
  ) {
    return ApiService.get(
      `/Account/GetChildrenData?admissiontypeid=${admissiontypeid}&identityDocumentId=${identityDocumentId}&documentSeries=${documentSeries}&documentNumber=${documentNumber}&dateOfBirth=${dateOfBirth}&lang=${lang}`
    );
  },
  CheckSMSCode(data) {
    return ApiService.post(`/Account/CheckSMSCode`, data);
  },
  ChangePassword(data) {
    return ApiService.post(`/Account/ChangePassword`, data);
  },
  Logout() {
    return ApiService.get("/Account/Logout");
  },
  RestorePassword(data) {
    return ApiService.post("/Account/RestorePassword", data);
  },

  RestorePasswordConfirm(data) {
    return ApiService.post("/Account/RestorePasswordConfirm", data);
  },
  GetParentForRegistration() {
    return ApiService.get("/Account/GetParentForRegistration");
  },
  GetChildrenForRegistration() {
    return ApiService.get("/Account/GetChildrenForRegistration");
  },
  Registrate(data) {
    return ApiService.post(`/Account/Registrate`, data);
  },
  GetContractorsList() {
    return ApiService.get(`/Account/GetContractorsList`);
  },
  SelectContractor(inn) {
    return ApiService.get(`/Account/SelectContractor?inn=${inn}`);
  },
  GetUserInfo() {
    return ApiService.get(`/Account/GetUserInfo`);
  },
  SetUserLanguage(data) {
    return ApiService.post(`/Account/SetUserLanguage`, data);
  },
  SyncWithSoliq() {
    return ApiService.get(`/Account/SyncWithSoliq`);
  },
  UpdateUserInfo(data) {
    return ApiService.post(`/Account/UpdateUserInfo`, data);
  },
  GetHash() {
    return ApiService.get(`/Account/GetHash`);
  },
  IsOffer(data) {
    return ApiService.post(`/Account/IsOffer`, data);
  },
  GetContractorMemshipState() {
    return ApiService.get(`/Account/GetContractorMemshipState`);
  },
  GetOrganizations(businessmanUserId) {
    return ApiService.get(
      `/Account/GetOrganizations?businessmanUserId=${businessmanUserId}`
    );
  },
  SetOrganization(data) {
    return ApiService.post(`/Account/SetOrganization`, data);
  },
  AddNewOrganization(data) {
    return ApiService.post(`/Account/AddNewOrganization`, data);
  },
  SearchByInnPnfl(innpnfl) {
    return ApiService.get(`/Account/SearchByInnPnfl?innpnfl=${innpnfl}`);
  },
  ChangePhoneNumber(data) {
    return ApiService.post(`/Account/ChangePhoneNumber`, data);
  },
  SendSMSCodeForChangePhoneNumber(data) {
    return ApiService.post(`/Account/SendSMSCodeForChangePhoneNumber`, data);
  },
  ChangeMainSettlementAccount(data) {
    return ApiService.post(`/Account/ChangeMainSettlementAccount`, data);
  },
  GetContractorSettlementAccountList() {
    return ApiService.post(`/Account/GetContractorSettlementAccountList`);
  },
  DeactivateAssociation(data) {
    return ApiService.post(`/Account/DeactivateAssociation`, data);
  },
};
export default AccountService;
