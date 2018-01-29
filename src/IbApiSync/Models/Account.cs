using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Models
{
    /// <summary>
    /// Class represanting all available information about IB account. Because I don't know what every value means, I won't
    /// comment properties here. Everybody should use properties about which he knows what they represent.
    /// </summary>
    public class Account
    {
        public string AccountCode { get; internal set; }
        public string AccountOrGroup { get; internal set; }
        public bool AccountReady { get; internal set; }
        public string AccountType { get; internal set; }
        public decimal AccruedCash { get; internal set; }
        public decimal AccruedCash_S { get; internal set; }
        public decimal AccruedCash_C { get; internal set; }
        public decimal AccruedCash_F { get; internal set; }
        public decimal AccruedDividend { get; internal set; }
        public decimal AccruedDividend_S { get; internal set; }
        public decimal AccruedDividend_C { get; internal set; }
        public decimal AccruedDividend_F { get; internal set; }
        public decimal AvailableFunds { get; internal set; }
        public decimal AvailableFunds_S { get; internal set; }
        public decimal AvailableFunds_C { get; internal set; }
        public decimal AvailableFunds_F { get; internal set; }
        public decimal Billable { get; internal set; }
        public decimal Billable_S { get; internal set; }
        public decimal Billable_C { get; internal set; }
        public decimal Billable_F { get; internal set; }
        public decimal BuyingPower { get; internal set; }
        public decimal CashBalance { get; internal set; }
        public decimal CorporateBondValue { get; internal set; }
        public string Currency { get; internal set; }
        public decimal Cushion { get; internal set; }
        public int DayTradesRemaining { get; internal set; }
        public int DayTradesRemainingT_1 { get; internal set; }
        public int DayTradesRemainingT_2 { get; internal set; }
        public int DayTradesRemainingT_3 { get; internal set; }
        public int DayTradesRemainingT_4 { get; internal set; }
        public decimal EquityWithLoanValue { get; internal set; }
        public decimal EquityWithLoanValue_S { get; internal set; }
        public decimal EquityWithLoanValue_C { get; internal set; }
        public decimal EquityWithLoanValue_F { get; internal set; }
        public decimal ExcessLiquidity { get; internal set; }
        public decimal ExcessLiquidity_S { get; internal set; }
        public decimal ExcessLiquidity_C { get; internal set; }
        public decimal ExcessLiquidity_F { get; internal set; }
        public decimal ExchangeRate { get; internal set; }
        public decimal FullAvailableFunds { get; internal set; }
        public decimal FullAvailableFunds_S { get; internal set; }
        public decimal FullAvailableFunds_C { get; internal set; }
        public decimal FullAvailableFunds_F { get; internal set; }
        public decimal FullExcessLiquidity { get; internal set; }
        public decimal FullExcessLiquidity_S { get; internal set; }
        public decimal FullExcessLiquidity_C { get; internal set; }
        public decimal FullExcessLiquidity_F { get; internal set; }
        public decimal FullInitMarginReq { get; internal set; }
        public decimal FullInitMarginReq_S { get; internal set; }
        public decimal FullInitMarginReq_C { get; internal set; }
        public decimal FullInitMarginReq_F { get; internal set; }
        public decimal FullMaintMarginReq { get; internal set; }
        public decimal FullMaintMarginReq_S { get; internal set; }
        public decimal FullMaintMarginReq_C { get; internal set; }
        public decimal FullMaintMarginReq_F { get; internal set; }
        public decimal FundValue { get; internal set; }
        public decimal FutureOptionValue { get; internal set; }
        public decimal FuturesPNL { get; internal set; }
        public decimal FxCashBalance { get; internal set; }
        public decimal GrossPositionValue { get; internal set; }
        public decimal GrossPositionValue_S { get; internal set; }
        public decimal GrossPositionValue_F { get; internal set; }
        public decimal Guarantee { get; internal set; }
        public decimal Guarantee_S { get; internal set; }
        public decimal Guarantee_C { get; internal set; }
        public decimal Guarantee_F { get; internal set; }
        public decimal IndianStockHaircut { get; internal set; }
        public decimal IndianStockHaircut_S { get; internal set; }
        public decimal IndianStockHaircut_C { get; internal set; }
        public decimal IndianStockHaircut_F { get; internal set; }
        public decimal InitMarginReq { get; internal set; }
        public decimal InitMarginReq_S { get; internal set; }
        public decimal InitMarginReq_C { get; internal set; }
        public decimal InitMarginReq_F { get; internal set; }
        public decimal IssuerOptionValue { get; internal set; }
        public decimal Leverage_S { get; internal set; }
        public decimal Leverage_F { get; internal set; }
        public decimal LookAheadNextChange { get; internal set; }
        public decimal LookAheadAvailableFunds { get; internal set; }
        public decimal LookAheadAvailableFunds_S { get; internal set; }
        public decimal LookAheadAvailableFunds_C { get; internal set; }
        public decimal LookAheadAvailableFunds_F { get; internal set; }
        public decimal LookAheadExcessLiquidity { get; internal set; }
        public decimal LookAheadExcessLiquidity_S { get; internal set; }
        public decimal LookAheadExcessLiquidity_C { get; internal set; }
        public decimal LookAheadExcessLiquidity_F { get; internal set; }
        public decimal LookAheadInitMarginReq { get; internal set; }
        public decimal LookAheadInitMarginReq_S { get; internal set; }
        public decimal LookAheadInitMarginReq_C { get; internal set; }
        public decimal LookAheadInitMarginReq_F { get; internal set; }
        public decimal LookAheadMaintMarginReq { get; internal set; }
        public decimal LookAheadMaintMarginReq_S { get; internal set; }
        public decimal LookAheadMaintMarginReq_C { get; internal set; }
        public decimal LookAheadMaintMarginReq_F { get; internal set; }
        public decimal MaintMarginReq { get; internal set; }
        public decimal MaintMarginReq_S { get; internal set; }
        public decimal MaintMarginReq_C { get; internal set; }
        public decimal MaintMarginReq_F { get; internal set; }
        public decimal MoneyMarketFundValue { get; internal set; }
        public decimal MutualFundValue { get; internal set; }
        public decimal NetDividend { get; internal set; }
        public decimal NetLiquidation { get; internal set; }
        public decimal NetLiquidation_S { get; internal set; }
        public decimal NetLiquidation_C { get; internal set; }
        public decimal NetLiquidation_F { get; internal set; }
        public decimal NetLiquidationByCurrency { get; internal set; }
        public decimal NetLiquidationUncertainty { get; internal set; }
        public decimal OptionMarketValue { get; internal set; }
        public decimal PASharesValue { get; internal set; }
        public decimal PASharesValue_S { get; internal set; }
        public decimal PASharesValue_C { get; internal set; }
        public decimal PASharesValue_F { get; internal set; }
        public decimal PostExpirationExcess { get; internal set; }
        public decimal PostExpirationExcess_S { get; internal set; }
        public decimal PostExpirationExcess_C { get; internal set; }
        public decimal PostExpirationExcess_F { get; internal set; }
        public decimal PostExpirationMargin { get; internal set; }
        public decimal PostExpirationMargin_S { get; internal set; }
        public decimal PostExpirationMargin_C { get; internal set; }
        public decimal PostExpirationMargin_F { get; internal set; }
        public decimal PreviousDayEquityWithLoanValue { get; internal set; }
        public decimal PreviousDayEquityWithLoanValue_S { get; internal set; }
        public string RealCurrency { get; internal set; }
        public decimal RealizedPnL { get; internal set; }
        public decimal RegTEquity { get; internal set; }
        public decimal RegTEquity_S { get; internal set; }
        public decimal RegTMargin { get; internal set; }
        public decimal RegTMargin_S { get; internal set; }
        public decimal SMA { get; internal set; }
        public decimal SMA_S { get; internal set; }
        public string SegmentTitle_S { get; internal set; }
        public string SegmentTitle_C { get; internal set; }
        public string SegmentTitle_F { get; internal set; }
        public decimal StockMarketValue { get; internal set; }
        public decimal TBondValue { get; internal set; }
        public decimal TBillValue { get; internal set; }
        public decimal TotalCashBalance { get; internal set; }
        public decimal TotalCashValue { get; internal set; }
        public decimal TotalCashValue_S { get; internal set; }
        public decimal TotalCashValue_C { get; internal set; }
        public decimal TotalCashValue_F { get; internal set; }
        public string TradingType_S { get; internal set; }
        public string TradingType_F { get; internal set; }
        public decimal UnrealizedPnL { get; internal set; }
        public decimal WarrantValue { get; internal set; }
        public bool WhatIfPMEnabled { get; internal set; }
    }
}
