namespace SspUis.Core;

public enum ExcludePerionType
{
    /// <summary>
    /// Ҳақ тўланадиган йиллик асосий таътил
    /// </summary>
    LeaveOrder,
    /// <summary>
    /// Иш берувчи билан келишувга кўра ходимга берилган иш ҳақи сақланмайдиган таътил
    /// </summary>
    LeaveOrderWithoutPay,
    /// <summary>
    /// Вақтинча меҳнатга лаёқатсизлик
    /// </summary>
    SickLeave,
    /// <summary>
    /// Ҳомиладорлик ва туғиш таътили
    /// </summary>
    SickLeaveIsMaternityLeave,
    /// <summary>
    /// Ўқиш (малака ошириш) муносабати билан ишлаб чиқаришдан ажралган ҳолда иш ҳақи сақланадиган таътил
    /// </summary>
    SendTrain
}
